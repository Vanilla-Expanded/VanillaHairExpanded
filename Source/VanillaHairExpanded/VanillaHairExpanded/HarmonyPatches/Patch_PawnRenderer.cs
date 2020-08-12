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

        [HarmonyPatch(typeof(PawnRenderer), "RenderPawnInternal", new Type[] { typeof(Vector3), typeof(float), typeof(bool), typeof(Rot4), typeof(Rot4), typeof(RotDrawMode), typeof(bool), typeof(bool), typeof(bool) })]
        public static class RenderPawnInternal
        {

            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator, MethodBase method)
            {
                var instructionList = instructions.ToList();
                bool hideHairDeclared = false;
                bool hideHairAssigned = false;
                bool hideHairChecked = false;
                bool drawBeardCall = false;

                var hideBeard = generator.DeclareLocal(typeof(bool));

                var getItemInfo = AccessTools.Method(typeof(List<ApparelGraphicRecord>), "get_Item");

                var getHatsOnlyOnMapInfo = AccessTools.Property(typeof(Prefs), nameof(Prefs.HatsOnlyOnMap)).GetGetMethod();

                var shouldHideHeadgearInfo = AccessTools.Method(typeof(RenderPawnInternal), nameof(ShouldHideHeadgear));
                var shouldHideBeardInfo = AccessTools.Method(typeof(RenderPawnInternal), nameof(ShouldHideBeard));
                var renderBeardInfo = AccessTools.Method(typeof(RenderPawnInternal), nameof(RenderBeard));

                for (int i = 0; i < instructionList.Count - 1; i++)
                {
                    var instruction = instructionList[i];

                    // Looking for calls to Prefs.HatsOnlyOnMap
                    if (instruction.opcode == OpCodes.Call && (MethodInfo)instruction.operand == getHatsOnlyOnMapInfo)
                    {
                        yield return instruction;
                        instruction = new CodeInstruction(OpCodes.Call, shouldHideHeadgearInfo);
                    }

                    // Looking for any assignments to 'bool flag'
                    if (instruction.operand is LocalBuilder lb && lb.LocalIndex == 14)
                    {
                        if (instruction.opcode == OpCodes.Stloc_S)
                        {
                            var prevInstruction = instructionList[i - 1];
                            if (!hideHairDeclared && prevInstruction.opcode == OpCodes.Ldc_I4_0)
                            {
                                yield return instruction; // bool flag = false
                                yield return new CodeInstruction(OpCodes.Ldc_I4_0); // false
                                instruction = new CodeInstruction(OpCodes.Stloc_S, hideBeard.LocalIndex); // hideBeard = false
                                hideHairDeclared = true;
                            }
                            if (!hideHairAssigned && prevInstruction.opcode == OpCodes.Ldc_I4_1)
                            {
                                yield return instruction; // flag = true
                                yield return new CodeInstruction(OpCodes.Ldloc_S, hideBeard.LocalIndex); // hideBeard
                                yield return new CodeInstruction(OpCodes.Ldloc_S, 5); // apparelGraphics
                                yield return new CodeInstruction(OpCodes.Ldloc_S, 16); // j
                                yield return new CodeInstruction(OpCodes.Callvirt, getItemInfo); // apparelGraphics[j]
                                yield return new CodeInstruction(OpCodes.Call, shouldHideBeardInfo); // ShouldHideBeard(hideBeard, apparelGraphics[j])
                                instruction = new CodeInstruction(OpCodes.Stloc_S, hideBeard.LocalIndex); // hideBeard = ShouldHideBeard(hideBeard, apparelGraphics[j])
                                hideHairAssigned = true;
                            }
                        }
                    }
                    if (!drawBeardCall && hideHairAssigned && instructionList[i - 7].opcode == OpCodes.Ldloc_S && instructionList[i - 7].operand is LocalBuilder llb && llb.LocalIndex == 14)
                    {
                        yield return new CodeInstruction(OpCodes.Ldarg_0); // this
                        yield return new CodeInstruction(OpCodes.Ldloc_S, hideBeard.LocalIndex); // hideBeard
                        yield return new CodeInstruction(OpCodes.Ldarg_S, 6);                    // bodyDrawType
                        yield return new CodeInstruction(OpCodes.Ldarg_S, 8);                    // headStump
                        yield return new CodeInstruction(OpCodes.Ldarg_S, 5);                    // headFacing
                        yield return new CodeInstruction(OpCodes.Ldloc_S, 13);                   // loc2
                        yield return new CodeInstruction(OpCodes.Ldloc_0);                       // quaternion
                        yield return new CodeInstruction(OpCodes.Ldarg_S, 7);                    // portrait
                        yield return new CodeInstruction(OpCodes.Call,    renderBeardInfo);      // RenderBeard(this, hideBeard, bodyDrawType, headStump, headFacing, loc2, quaternion, portrait)
                        drawBeardCall = true;
                    }


                    yield return instruction;
                }

                var lastInstruction = instructionList.Last();

                yield return lastInstruction;
            }

            private static bool ShouldHideHeadgear(bool original)
            {
                // Also check if there are any change hairstyle windows currently open
                return original || Find.WindowStack.Windows.Any(w => w is Dialog_ChangeHairstyle);
            }

            private static bool ShouldHideBeard(bool hidden, ApparelGraphicRecord graphicRecord)
            {
                // Check if apparel covers any parts that should hide beards
                if (!hidden)
                {
                    return graphicRecord.sourceApparel.def.apparel.bodyPartGroups.Any(g => BodyPartGroupDefExtension.Get(g).hideBeardIfCovered);
                }
                return hidden;
            }

            private static void RenderBeard(PawnRenderer instance, bool hideBeard, RotDrawMode bodyDrawType, bool headStump, Rot4 headFacing, Vector3 baseDrawPos, Quaternion quaternion, bool portrait)
            {
                var pawn = instance.graphics.pawn;
                if (VanillaHairExpandedSettings.beards && !hideBeard && bodyDrawType != RotDrawMode.Dessicated && !headStump &&
                    pawn.GetComp<CompBeard>() is CompBeard beardComp && beardComp.beardDef != null)
                {
                    var beardMesh = instance.graphics.HairMeshSet.MeshAt(headFacing);
                    var beardMat = instance.graphics.BeardMatAt(headFacing);
                    GenDraw.DrawMeshNowOrLater(beardMesh, baseDrawPos - new Vector3(0, 0.00005f, 0), quaternion, beardMat, portrait);
                }
            }

        }

    }

}
