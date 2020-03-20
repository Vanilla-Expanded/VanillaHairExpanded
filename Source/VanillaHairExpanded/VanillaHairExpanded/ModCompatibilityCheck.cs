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
    public static class ModCompatibilityCheck
    {

        public static bool EdBPrepareCarefully = ModsConfig.ActiveModsInLoadOrder.Any(m => m.Name == "EdB Prepare Carefully");

        public static bool HumanoidAlienRaces = ModsConfig.ActiveModsInLoadOrder.Any(m => m.Name == "Humanoid Alien Races 2.0");

    }

}
