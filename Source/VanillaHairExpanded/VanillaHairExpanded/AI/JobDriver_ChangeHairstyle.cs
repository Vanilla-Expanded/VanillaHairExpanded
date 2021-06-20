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

    [DefOf]
    public class JobDriver_ChangeHairstyle : JobDriver
    {

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(job.GetTarget(TableIndex), job, errorOnFailed: errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            // Go to hairdressing table
            var gotoToil = Toils_Goto.GotoThing(TableIndex, PathEndMode.InteractionCell);
            yield return gotoToil;

            // Bring up interface
            yield return new Toil()
            {
                initAction = () =>
                {
                    Find.WindowStack.Add(new Dialog_ChangeHairstyle(this));
                },
                defaultCompleteMode = ToilCompleteMode.Never
            };

            // Change hairstyle
            var hairdressToil = new Toil
            {
                tickAction = () =>
                {
                    // Work on changing hairstyle
                    restyleTicksDone += pawn.GetStatValue(RimWorld.StatDefOf.GeneralLaborSpeed);
                    if (restyleTicksDone >= ticksToRestyle)
                    {
                        if (AnyChanges)
                            FilthMaker.TryMakeFilth(pawn.Position, pawn.Map, ThingDefOf.VHE_Filth_Hair, 3);

                        if (newHairDef != null)
                            pawn.story.hairDef = newHairDef;
                        if (newHairColour.HasValue)
                            pawn.story.hairColor = newHairColour.Value;

                        if (newBeardDef != null)
                            this.pawn.style.beardDef = newBeardDef;
                        
                        pawn.Drawer.renderer.graphics.ResolveAllGraphics();
                        PortraitsCache.SetDirty(pawn);
                        GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(pawn);
                        pawn.jobs.EndCurrentJob(JobCondition.Succeeded);
                    }
                },
                defaultCompleteMode = ToilCompleteMode.Never
            };
            hairdressToil.WithProgressBar(TableIndex, () => restyleTicksDone / ticksToRestyle, true);
            hairdressToil.FailOnCannotTouch(TableIndex, PathEndMode.Touch);
            hairdressToil.PlaySustainerOrSound(SoundDefOf.Recipe_Tailor);
            yield return hairdressToil;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look(ref newHairDef, "newHairDef");
            Scribe_Values.Look(ref newHairColour, "newHairColour");
            Scribe_Defs.Look(ref newBeardDef, "newBeardDef");
            Scribe_Values.Look(ref ticksToRestyle, "ticksToRestyle");
            Scribe_Values.Look(ref restyleTicksDone, "restyleTicksDone");
        }

        private const TargetIndex TableIndex = TargetIndex.A;

        private bool AnyChanges => newHairDef != null || newHairColour.HasValue || newBeardDef != null;

        public HairDef newHairDef;
        public Color? newHairColour;
        public BeardDef newBeardDef;
        public int ticksToRestyle;
        private float restyleTicksDone;

    }

}
