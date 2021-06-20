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

    public class VanillaHairExpanded : Mod
    {

        public VanillaHairExpanded(ModContentPack content) : base(content)
        {
            harmonyInstance = new Harmony("OskarPotocki.VanillaHairExpanded");
        }

        public static Harmony harmonyInstance;

    }

}
