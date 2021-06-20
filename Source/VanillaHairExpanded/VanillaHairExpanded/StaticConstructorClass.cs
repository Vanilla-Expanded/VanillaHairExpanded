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
            var thingDefs = DefDatabase<ThingDef>.AllDefsListForReading;
            for (int i = 0; i < thingDefs.Count; i++)
            {
                var tDef = thingDefs[i];
                // Add beard comps to all eligible ThingDefs
                if (typeof(Pawn).IsAssignableFrom(tDef.thingClass) && tDef.race != null && tDef.race.Humanlike)
                {
                    if (tDef.comps == null)
                        tDef.comps = new List<CompProperties>();
                }
            }
        }

        private static void ResolveErroredHairDefs()
        {
            var hairDefs = DefDatabase<HairDef>.AllDefsListForReading;
            for (int i = 0; i < hairDefs.Count; i++)
            {
                var hDef = hairDefs[i];
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
