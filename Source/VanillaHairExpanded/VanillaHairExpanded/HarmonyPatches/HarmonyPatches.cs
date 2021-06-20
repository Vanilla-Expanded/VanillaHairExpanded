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

    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {

        static HarmonyPatches()
        {
            //HarmonyInstance.DEBUG = true;

            //Harmony.DEBUG = true;
            VanillaHairExpanded.harmonyInstance.PatchAll();
            return;
            /*
            // PawnHairChooser.RandomHairDefFor anonymous
            VanillaHairExpanded.harmonyInstance.Patch(original: typeof(PawnHairChooser).GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Instance).
                First(t => t.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).Any(m => m.Name.Contains("RandomHairDefFor"))).
                GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).First(m => m.ReturnType == typeof(bool)),
                prefix: new HarmonyMethod(typeof(Patch_PawnHairChooser.manual_RandomHairDefFor), "Prefix"));*/
        }

    }

}
