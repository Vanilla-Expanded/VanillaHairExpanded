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

    public static class PawnGraphicSet_Ext
    {

        public static Material BeardMatAt(this PawnGraphicSet instance, Rot4 facing)
        {
            if (instance.BeardGraphic() is Graphic beardGraphic)
            {
                var baseMat = beardGraphic.MatAt(facing);
                return instance.flasher.GetDamagedMat(baseMat);
            }
            return null;
        }

        public static Graphic BeardGraphic(this PawnGraphicSet instance)
        {
            return instance.pawn.Drawer.renderer.graphics.beardGraphic;
        }

    }

}
