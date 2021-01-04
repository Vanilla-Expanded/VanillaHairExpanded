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

    public class PawnKindDefExtension : DefModExtension
    {
        public float beardFactor = 1f;

        private static readonly PawnKindDefExtension defaultValues = new PawnKindDefExtension();

        public static PawnKindDefExtension Get(Def def) => def.GetModExtension<PawnKindDefExtension>() ?? defaultValues;

        public float BeardChanceFor(Pawn pawn)
        {
            // Race-based (if applicable) -- return 0 if race is set to not have hair
            if (ModCompatibilityCheck.HumanoidAlienRaces &&
                NonPublicTypes.HumanoidAlienRaces.ThingDef_AlienRace.IsAssignableFrom(pawn.def.GetType()) &&
                NonPublicFields.HumanoidAlienRaces.ThingDef_AlienRace_alienRace.GetValue(pawn.def) is object alienSettings &&
                NonPublicFields.HumanoidAlienRaces.AlienSettings_hairSettings.GetValue(alienSettings) is object hairSettings &&
                !(bool)NonPublicFields.HumanoidAlienRaces.HairSettings_hasHair.GetValue(hairSettings))
            {
                return 0;
            }

            // Gender-based
            return BeardChanceFor(pawn.gender) * this.beardFactor;
        }

        public float BeardChanceFor(Gender gender)
        {
            if (genderBeardChances.TryGetValue(gender, out float chance))
                return chance;
            return 0;
        }

        private Dictionary<Gender, float> genderBeardChances = new Dictionary<Gender, float>()
        {
            { Gender.Male, 0.2f }
        };

    }

}
