using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;
using RimWorld;
using Verse;

namespace HaremLeaderMeme
{
	public class HaremLeaderMemeMod : Mod
	{
		public HaremLeaderMemeMod(ModContentPack content) : base(content)
		{
			// WBR requires Harmony, so Harmony is only touched when WBR is loaded; without WBR this mod stays Harmony-free.
			if (ModsConfig.IsActive("divineDerivative.Romance"))
			{
				WayBetterRomanceCompat.Apply();
			}
		}
	}

	// Way Better Romance decides whose partners count as cheated on with RomanceUtilities.WillingToDoGendered, which only
	// scans the partner's ideo for a matching PreceptComp_UnwillingToDo_Gendered and never calls MemberWillingToDo, so the
	// leader exemption is invisible to it. Every romance or hookup by the harem leader then gives their same-ideo partners
	// CheatedOnMe. This postfix drops those partners from the cheated-on list; partners of other ideos still count.
	internal static class WayBetterRomanceCompat
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void Apply()
		{
			MethodInfo target = AccessTools.Method("BetterRomance.RomanceUtilities:IsThisCheating");
			if (target == null)
			{
				Log.Warning("[Leader's Harem] Way Better Romance is active but RomanceUtilities.IsThisCheating was not found; harem partners may still feel cheated on.");
				return;
			}
			new Harmony("DanZinagri.HaremLeaderMeme").Patch(target, postfix: new HarmonyMethod(typeof(WayBetterRomanceCompat), nameof(IsThisCheating_Postfix)));
		}

		public static void IsThisCheating_Postfix(Pawn pawn, ref List<Pawn> cheaterList, ref bool __result)
		{
			if (cheaterList == null || cheaterList.Count == 0 || !PreceptComp_UnwillingToDo_Gendered_LeaderExempt.IsExemptIdeoLeader(pawn))
			{
				return;
			}
			Ideo ideo = pawn.Ideo;
			cheaterList.RemoveAll(p => p.Ideo == ideo);
			// The exempt leader always passes WBR's own willingness check, so the only remaining "true" path is
			// "someone was cheated on and the pawn is Faithful". With nobody left on the list, that is false.
			if (cheaterList.Count == 0)
			{
				__result = false;
			}
		}
	}
}
