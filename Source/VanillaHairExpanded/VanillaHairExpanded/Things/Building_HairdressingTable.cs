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

    public class Building_HairdressingTable : Building
    {

        public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
        {
            if (selPawn.IsColonistPlayerControlled)
            {
                yield return new FloatMenuOption("VanillaHairExpanded.ChangeHairstyle".Translate(),
                    () => selPawn.jobs.TryTakeOrderedJob(new Job(JobDefOf.VHE_ChangeHairstyle, this)));
            }
        }

    }

}
