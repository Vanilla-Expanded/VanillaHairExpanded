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

    public static class Patch_EdBPrepareCarefully_ProviderHair
    {

        public static class manual_GetHairs
        {

            public static void Postfix(ref List<HairDef> __result)
            {
                // Exclude beards from the list of hairs to choose from
                __result = __result.Where(h => !HairDefExtension.Get(h).isBeard).ToList();
            }

        }

    }

}
