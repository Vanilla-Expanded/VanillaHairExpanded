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

    public class BodyPartGroupDefExtension : DefModExtension
    {

        private static readonly BodyPartGroupDefExtension defaultValues = new BodyPartGroupDefExtension();

        public static BodyPartGroupDefExtension Get(Def def) => def.GetModExtension<BodyPartGroupDefExtension>() ?? defaultValues;

        public bool hideBeardIfCovered = false;

    }

}
