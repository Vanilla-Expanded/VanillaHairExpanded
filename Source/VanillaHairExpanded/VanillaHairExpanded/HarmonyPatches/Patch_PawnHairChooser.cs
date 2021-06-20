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

    public static class Patch_PawnHairChooser
    {

        public static class manual_RandomHairDefFor
        {

            public static bool Prefix(HairDef hair, ref bool __result)
            {
                var extension = HairDefExtension.Get(hair);

                // Don't include non-random or beards in the list of possible hairstyles
                if (!extension.randomlySelectable)
                {
                    __result = false;
                    return false;
                }
                return true;
            }

        }

    }

}
