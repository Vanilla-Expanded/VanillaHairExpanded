using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using HarmonyLib;

namespace VanillaHairExpanded
{

    public static class Patch_PawnRenderer
    {

        [HarmonyPatch]
        public static class RenderPawnInternal
        {
            [HarmonyTargetMethods]
            public static IEnumerable<MethodBase> TargetMethods()
            {
                yield return AccessTools.Method(typeof(PawnRenderNodeWorker_Apparel_Head), nameof(PawnRenderNodeWorker_Apparel_Head.CanDrawNow));
                yield return AccessTools.Method(typeof(PawnRenderNodeWorker_Apparel_Head), nameof(PawnRenderNodeWorker_Apparel_Head.HeadgearVisible));
            }

            [HarmonyTranspiler]
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator, MethodBase method)
            {
                var instructionList = instructions.ToList();
                bool hideHairDeclared = false;

                var getHatsOnlyOnMapInfo = AccessTools.PropertyGetter(typeof(Prefs), nameof(Prefs.HatsOnlyOnMap));

                var shouldHideHeadgearInfo = AccessTools.Method(typeof(RenderPawnInternal), nameof(ShouldHideHeadgear));

                for (int i = 0; i < instructionList.Count; i++)
                {
                    var instruction = instructionList[i];

                    // Looking for calls to Prefs.HatsOnlyOnMap
                    if (instruction.Calls(getHatsOnlyOnMapInfo))
                    {
                        yield return instruction;
                        instruction = new CodeInstruction(OpCodes.Call, shouldHideHeadgearInfo);
                    }

                    yield return instruction;
                }
            }

            private static bool ShouldHideHeadgear(bool original)
            {
                // Also check if there are any change hairstyle windows currently open
                return original || Find.WindowStack.Windows.Any(w => w is Dialog_ChangeHairstyle);
            }

        }

    }

}
