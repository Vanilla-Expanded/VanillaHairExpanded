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

    public static class PawnBeardChooser
    {

        public static void GenerateBeard(Pawn pawn, FactionDef faction)
        {
            if (pawn.GetComp<CompBeard>() is CompBeard beardComp)
            {
                beardComp.beardColour = pawn.story.hairColor;
                if (beardComp.CanRandomlyGenerateBeard && Rand.Chance(PawnKindDefExtension.Get(pawn.kindDef).BeardChanceFor(pawn)))
                    beardComp.beardDef = RandomBeardDefFor(pawn, faction);
                else
                    beardComp.beardDef = HairDefOf.VHE_BeardCleanShaven;
            }
            else
            {
                Log.Warning($"{pawn} (def={pawn.def}) is capable of having hairstyles but has null beardComp.");
            }
        }

        public static HairDef RandomBeardDefFor(Pawn pawn, FactionDef factionType)
        {
            var beards = DefDatabase<HairDef>.AllDefs.Where(h => 
            {
                // Determine hair tags
                var extension = HairDefExtension.Get(h);
                var backstoryHairTags = NonPublicMethods.PawnHairChooser_HairTagsFromBackstory(pawn);
                var pawnKindHairTags = NonPublicMethods.PawnHairChooser_HairTagsFromPawnKind(pawn);
                var chosenTags = pawnKindHairTags.Any() ? pawnKindHairTags : backstoryHairTags;
                if (!chosenTags.Any())
                    chosenTags = factionType.hairTags;

                return extension.randomlySelectable && extension.isBeard && h.hairTags.SharesElementWith(chosenTags);
            });
            return beards.Any() ? beards.RandomElementByWeight(h => NonPublicMethods.PawnHairChooser_HairChoiceLikelihoodFor(h, pawn)) : HairDefOf.VHE_BeardCleanShaven;
        }

    }

}
