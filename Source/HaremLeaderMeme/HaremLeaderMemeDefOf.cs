using RimWorld;
using Verse;

namespace HaremLeaderMeme
{
	[DefOf]
	public static class HaremLeaderMemeDefOf
	{
		public static MemeDef LeadersHarem;

		static HaremLeaderMemeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(HaremLeaderMemeDefOf));
		}
	}
}
