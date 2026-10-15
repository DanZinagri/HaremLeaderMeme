using RimWorld;
using Verse;

namespace HaremLeaderMeme
{
	[DefOf]
	public static class HaremLeaderMemeDefOf
	{
		public static MemeDef LeadersHarem;

		public static IssueDef HaremLeaderMeme_HaremScope;

		public static PreceptDef HaremLeaderMeme_HaremScope_FirstLeader;
		public static PreceptDef HaremLeaderMeme_HaremScope_AllLeaders;
		public static PreceptDef HaremLeaderMeme_HaremScope_AllRoles;
		public static PreceptDef HaremLeaderMeme_HaremScope_AllRolesButMoralGuide;

		static HaremLeaderMemeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(HaremLeaderMemeDefOf));
		}
	}
}
