using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using HarmonyLib;

namespace VanillaHairExpanded
{

    [StaticConstructorOnStartup]
    public static class StaticConstructorClass
    {

        static StaticConstructorClass()
        {
            PatchThingDefs();
            ResolveErroredHairDefs();
        }

        private static void PatchThingDefs()
        {
            foreach (var tDef in DefDatabase<ThingDef>.AllDefs)
            {
                // Add beard comps to all eligible ThingDefs
                if (typeof(Pawn).IsAssignableFrom(tDef.thingClass) && tDef.race != null && tDef.race.Humanlike)
                {
                    if (tDef.comps == null)
                        tDef.comps = new List<CompProperties>();
                    if (!tDef.comps.Any(c => typeof(CompProperties_Beard).IsAssignableFrom(c.GetType())))
                        tDef.comps.Add(new CompProperties_Beard());
                }
            }
        }

        private static void ResolveErroredHairDefs()
        {
            foreach (var hDef in DefDatabase<HairDef>.AllDefsListForReading)
            {
                if (ContentFinder<Texture2D>.Get(hDef.texPath + "_north", false) == null && ContentFinder<Texture2D>.Get(hDef.texPath + "_south", false) == null &&
                    ContentFinder<Texture2D>.Get(hDef.texPath + "_east", false) == null && ContentFinder<Texture2D>.Get(hDef.texPath + "_west", false) == null)
                    badHairDefs.Add(hDef);
            }
            if (badHairDefs.Any())
            {
                var errorStrings = new List<string>();
                badHairDefs.ForEach(h => errorStrings.Add($"{h.defName} (from {h.modContentPack?.Name ?? "unknown source"})"));
                Log.Error($"The following HairDefs have misconfigured texPaths or missing textures: {errorStrings.ToCommaList()}");
            }
        }

        public static List<HairDef> badHairDefs = new List<HairDef>();

    }

}
