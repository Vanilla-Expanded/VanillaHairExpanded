using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using Harmony;

namespace VanillaHairExpanded
{

    [StaticConstructorOnStartup]
    public static class DefPatcher
    {

        static DefPatcher()
        {
            PatchThingDefs();
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

    }

}
