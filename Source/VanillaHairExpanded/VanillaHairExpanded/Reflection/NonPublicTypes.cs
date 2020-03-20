using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using HarmonyLib;

namespace VanillaHairExpanded
{

    public static class NonPublicTypes
    {

        [StaticConstructorOnStartup]
        public static class HumanoidAlienRaces
        {

            static HumanoidAlienRaces()
            {
                // Add types from humanoid alien races
                if (ModCompatibilityCheck.HumanoidAlienRaces)
                {
                    ThingDef_AlienRace = GenTypes.GetTypeInAnyAssembly("AlienRace.ThingDef_AlienRace", "AlienRace");
                    AlienSettings = ThingDef_AlienRace.GetNestedType("AlienSettings", BindingFlags.Public | BindingFlags.Instance);
                    HairSettings = GenTypes.GetTypeInAnyAssembly("AlienRace.HairSettings", "AlienRace");
                }
            }

            public static Type ThingDef_AlienRace;
            public static Type AlienSettings;
            public static Type HairSettings;

        }

    }

}
