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

    public class CompProperties_Beard : CompProperties
    {

        public CompProperties_Beard()
        {
            compClass = typeof(CompBeard);
        }

        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            if (parentDef.race == null)
                yield return "parentDef has null RaceProperties.";
            else if (parentDef.race.lifeStageAges.NullOrEmpty())
                yield return "parentDef has null or empty lifeStageAges in race.";
            else if (!parentDef.race.lifeStageAges.Any(a => a.def == minLifeStage))
                yield return $"minLifeStage={minLifeStage} but parentDef has no lifeStageAges with any defs that match minLifeStage.";
        }

        public LifeStageDef minLifeStage = LifeStageDefOf.HumanlikeAdult;

    }

}
