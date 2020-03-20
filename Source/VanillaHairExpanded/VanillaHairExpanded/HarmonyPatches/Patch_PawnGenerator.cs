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

    public static class Patch_PawnGenerator
    {

        [HarmonyPatch(typeof(PawnGenerator), "TryGenerateNewPawnInternal")]
        public static class TryGenerateNewPawnInternal
        {

            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase method)
            {
                var instructionList = instructions.ToList();
                bool done = false;

                int factionDefLocalIndex = method.GetMethodBody().LocalVariables.First(l => l.LocalType == typeof(FactionDef)).LocalIndex;

                var hairDefInfo = AccessTools.Field(typeof(Pawn_StoryTracker), nameof(Pawn_StoryTracker.hairDef));

                var generateBeardInfo = AccessTools.Method(typeof(PawnBeardChooser), nameof(PawnBeardChooser.GenerateBeard));

                for (int i = 0; i < instructionList.Count; i++)
                {
                    var instruction = instructionList[i];

                    // Add our beard generation code after the hair has been generated
                    if (!done && instruction.opcode == OpCodes.Stfld && instruction.OperandIs(hairDefInfo))
                    {
                        yield return instruction; // pawn.story.hairDef = PawnHairChooser.RandomHairDefFor(pawn, def)
                        yield return new CodeInstruction(OpCodes.Ldloc_0); // pawn
                        yield return new CodeInstruction(OpCodes.Ldloc_S, factionDefLocalIndex); // def
                        instruction = new CodeInstruction(OpCodes.Call, generateBeardInfo); // GenerateBeard(pawn, def)
                        done = true;
                    }

                    yield return instruction;
                }
            }

        }

    }

}
