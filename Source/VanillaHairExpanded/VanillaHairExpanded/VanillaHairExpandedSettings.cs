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

    public class VanillaHairExpandedSettings : ModSettings
    {

        public void DoWindowContents(Rect wrect)
        {
            var options = new Listing_Standard();
            var defaultColor = GUI.color;
            options.Begin(wrect);
            GUI.color = defaultColor;
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.UpperLeft;

            // Beards
            options.Gap();
            options.CheckboxLabeled("VanillaHairExpanded.Beards".Translate(), ref beards, "VanillaHairExpanded.Beards_ToolTip".Translate());

            options.End();
            Write();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref beards, "beards", true);
        }

        public static bool beards = true;

    }

}
