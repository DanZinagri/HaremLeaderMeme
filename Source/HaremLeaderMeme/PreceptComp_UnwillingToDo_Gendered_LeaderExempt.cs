using System.Collections.Generic;
using RimWorld;
using Verse;

namespace HaremLeaderMeme
{
	// Identical to the vanilla PreceptComp_UnwillingToDo_Gendered, except role holders covered by the ideo's harem privilege precept ignore it
	// when that ideo has the LeadersHarem meme. Everyone else, and ideos without the meme, fall through to the normal vanilla check.
	public class PreceptComp_UnwillingToDo_Gendered_LeaderExempt : PreceptComp_UnwillingToDo_Gendered
	{
		public override bool MemberWillingToDo(HistoryEvent ev)
		{
			// Same cheap eventDef check the base class does - bail out before touching Ideo/meme/role lookups, since this runs against every HistoryEvent, not just marriage ones.
			if (eventDef != null && ev.def != eventDef)
			{
				return true;
			}
			if (ev.args.TryGetArg(HistoryEventArgsNames.Doer, out Pawn doer) && IsExemptIdeoLeader(doer))
			{
				return true;
			}
			return base.MemberWillingToDo(ev);
		}

		private static bool IsExemptIdeoLeader(Pawn pawn)
		{
			Ideo ideo = pawn?.Ideo;
			if (ideo == null || !ideo.HasMeme(HaremLeaderMemeDefOf.LeadersHarem))
			{
				return false;
			}
			Precept_Role role = ideo.GetRole(pawn);
			if (role == null)
			{
				return false;
			}
			// Every mode covers the first leader.
			if (role.def.leaderRole)
			{
				return true;
			}
			PreceptDef scope = GetHaremScope(ideo);
			List<string> tags = role.def.roleTags;
			if (scope == HaremLeaderMemeDefOf.HaremLeaderMeme_HaremScope_AllRoles)
			{
				return true;
			}
			if (scope == HaremLeaderMemeDefOf.HaremLeaderMeme_HaremScope_AllRolesButMoralGuide)
			{
				return tags == null || !tags.Contains("Moralist");
			}
			if (scope == HaremLeaderMemeDefOf.HaremLeaderMeme_HaremScope_AllLeaders)
			{
				return tags != null && tags.Contains("Leader");
			}
			return false;
		}

		// Ideos saved before the harem privilege issue existed have no precept for it; treat that as first leader only.
		private static PreceptDef GetHaremScope(Ideo ideo)
		{
			List<Precept> precepts = ideo.PreceptsListForReading;
			for (int i = 0; i < precepts.Count; i++)
			{
				if (precepts[i].def.issue == HaremLeaderMemeDefOf.HaremLeaderMeme_HaremScope)
				{
					return precepts[i].def;
				}
			}
			return HaremLeaderMemeDefOf.HaremLeaderMeme_HaremScope_FirstLeader;
		}
	}
}
