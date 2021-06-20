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

        [HarmonyPatch(typeof(PawnRenderer), "DrawHeadHair")]
        public static class RenderPawnInternal
        {

            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator, MethodBase method)
            {
                var instructionList = instructions.ToList();
                bool hideHairDeclared = false;

                var getHatsOnlyOnMapInfo = AccessTools.Property(typeof(Prefs), nameof(Prefs.HatsOnlyOnMap)).GetGetMethod();

                var shouldHideHeadgearInfo = AccessTools.Method(typeof(RenderPawnInternal), nameof(ShouldHideHeadgear));

                for (int i = 0; i < instructionList.Count; i++)
                {
                    var instruction = instructionList[i];

                    // Looking for calls to Prefs.HatsOnlyOnMap
                    if (instruction.opcode == OpCodes.Call && (MethodInfo)instruction.operand == getHatsOnlyOnMapInfo)
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
