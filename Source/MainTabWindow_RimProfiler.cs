using System;
using System.Collections.Generic;
using Verse;
using RimWorld;
using UnityEngine;

namespace RimProfiler
{
    public class MainTabWindow_RimProfiler : MainTabWindow
    {
        public override Vector2 RequestedTabSize => new Vector2(1000f, 500f);

        public override void DoWindowContents(Rect inRect)
        {
            GUI.BeginGroup(inRect);

            DoThingTab(inRect);//TODO - add tabs

            GUI.EndGroup();
        }

        public override void PreOpen()
        {
            base.PreOpen();
            RimProfiler.EntityMeasurer.StartProfiling();
        }

        public override void PostClose()
        {
            base.PostClose();
            RimProfiler.EntityMeasurer.StopProfiling();
        }

        private void DoThingTab(Rect rect)
        {
            GUI.BeginGroup(rect);

            Listing_Standard listing = new Listing_Standard();
            listing.Begin(rect);

            foreach (var measurement in RimProfiler.EntityMeasurer.Measurements)
            {
                listing.Label(string.Format("{0}   {1}", measurement.Value, measurement.Key));
            }

            listing.End();
            GUI.EndGroup();
        }
    }
}
