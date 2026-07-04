using RimWorld;
using Verse;

namespace HaremLeaderMeme
{
	// Identical to the vanilla PreceptComp_UnwillingToDo_Gendered, except the leader can ignore normal precept restrictions.
	// that ideo has the LeadersHarem meme. Everyone else, and leaders of ideos without the meme, fall through to the normal vanilla check.
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
			return role != null && role.def.leaderRole;
		}
	}
}
