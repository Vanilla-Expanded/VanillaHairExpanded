using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using RimWorld;
using HarmonyLib;

namespace VanillaHairExpanded
{

    public class HairDefExtension : DefModExtension
    {

        private static readonly HairDefExtension defaultValues = new HairDefExtension();

        public static HairDefExtension Get(Def def) => def.GetModExtension<HairDefExtension>() ?? defaultValues;

        public bool randomlySelectable = true;
        public int workToStyle = 300;

    }

}
