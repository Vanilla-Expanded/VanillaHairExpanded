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

    public static class NonPublicFields
    {

        [StaticConstructorOnStartup]
        public static class HumanoidAlienRaces
        {

            static HumanoidAlienRaces()
            {
                // Add fields from humanoid alien races
                if (ModCompatibilityCheck.HumanoidAlienRaces)
                {
                    ThingDef_AlienRace_alienRace = AccessTools.Field(NonPublicTypes.HumanoidAlienRaces.ThingDef_AlienRace, "alienRace");
                    AlienSettings_hairSettings = AccessTools.Field(NonPublicTypes.HumanoidAlienRaces.AlienSettings, "hairSettings");
                    HairSettings_hasHair = AccessTools.Field(NonPublicTypes.HumanoidAlienRaces.HairSettings, "hasHair");
                }
            }

            public static FieldInfo ThingDef_AlienRace_alienRace;
            public static FieldInfo AlienSettings_hairSettings;
            public static FieldInfo HairSettings_hasHair;

        }

    }

}
