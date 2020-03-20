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

    public static class Patch_PawnGraphicSet
    {

        [HarmonyPatch(typeof(PawnGraphicSet), nameof(PawnGraphicSet.ResolveAllGraphics))]
        public static class ResolveAllGraphics
        {

            public static void Postfix(PawnGraphicSet __instance)
            {
                // Resolve beard graphics
                if (__instance.pawn.RaceProps.Humanlike && __instance.pawn.GetComp<CompBeard>() is CompBeard beardComp && beardComp.beardDef != null)
                {
                    beardComp.beardGraphic = GraphicDatabase.Get<Graphic_Multi>(beardComp.beardDef.texPath, ShaderDatabase.Cutout, Vector2.one, beardComp.beardColour);
                }
            }

        }

    }

}
