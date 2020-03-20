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

    [StaticConstructorOnStartup]
    public static class NonPublicMethods
    {

        public static Func<HairDef, Pawn, float> PawnHairChooser_HairChoiceLikelihoodFor = (Func<HairDef, Pawn, float>)
            Delegate.CreateDelegate(typeof(Func<HairDef, Pawn, float>), AccessTools.Method(typeof(PawnHairChooser), "HairChoiceLikelihoodFor"));
        public static Func<Pawn, IEnumerable<string>> PawnHairChooser_HairTagsFromBackstory = (Func<Pawn, IEnumerable<string>>)
            Delegate.CreateDelegate(typeof(Func<Pawn, IEnumerable<string>>), typeof(PawnHairChooser).GetMethod("HairTagsFromPawnKind", BindingFlags.NonPublic | BindingFlags.Static));
        public static Func<Pawn, IEnumerable<string>> PawnHairChooser_HairTagsFromPawnKind = (Func<Pawn, IEnumerable<string>>)
            Delegate.CreateDelegate(typeof(Func<Pawn, IEnumerable<string>>), AccessTools.Method(typeof(PawnHairChooser), "HairTagsFromBackstory"));

    }

}
