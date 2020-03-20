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

    public class CompBeard : ThingComp
    {

        private CompProperties_Beard Props => (CompProperties_Beard)props;

        private Pawn Pawn => (Pawn)parent;

        public bool CanRandomlyGenerateBeard
        {
            get
            {
                int minIndexForBeard = Pawn.RaceProps.lifeStageAges.FirstIndexOf(a => a.def == Props.minLifeStage);
                return Pawn.ageTracker.CurLifeStageIndex >= minIndexForBeard;
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);

            // Save compatibility
            if (beardDef == null)
                PawnBeardChooser.GenerateBeard(Pawn, Pawn.Faction?.def);
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref beardColour, "beardColour");
            Scribe_Defs.Look(ref beardDef, "beardDef");
        }

        public Color beardColour;
        public HairDef beardDef;

        public Graphic beardGraphic;

    }

}
