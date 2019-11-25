using System;
using System.Collections.Generic;
using Verse;
using RimWorld;
using UnityEngine;

namespace RimProfiler
{
    public class MainTabWindow_RimProfiler : MainTabWindow
    {

        protected virtual float ExtraBottomSpace => 53f;
        protected virtual float ExtraTopSpace => 0f;
        protected override float Margin => 6f;




        private Vector2 scrollPosition;

        private readonly Vector2 size = new Vector2(1000f, 500f); // TODO - calculate this

        public override Vector2 RequestedTabSize
        {
            get
            {
                return new Vector2(size.x + Margin * 2f, size.y + ExtraBottomSpace + ExtraTopSpace + Margin * 2f);
            }
        }

        public override void DoWindowContents(Rect inRect)
        {
            GUI.BeginGroup(inRect);

            //TODO - add tabs
            DoThingTab(inRect.ContractedBy(Margin)
                    .BottomPart(ExtraTopSpace)
                    .TopPart(ExtraBottomSpace));

            GUI.EndGroup();
        }

        public override void PreOpen()
        {
            RimProfiler.EntityMeasurer.StartProfiling();
            base.PreOpen();
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
                var result = measurement.Value;
                listing.Label(string.Format("{0,-16}  {1,10:N2}    {2}", result.Duration, result.Invocations, measurement.Key));
            }

            listing.End();
            GUI.EndGroup();



            float cachedHeightNoScrollbar = 3f;//TODO
            float cachedHeaderHeight = 3f;//TODO


            // TODO
            // Draw Header
            float viewportWidth = size.x - 16f;
            int currentPosition = 0;
            for (int i = 0; i < def.columns.Count; i++)
            {
                int num3 = (i != def.columns.Count - 1) ? ((int)cachedColumnWidths[i]) : ((int)(viewportWidth - (float)currentPosition));
                Rect headerRect = new Rect(Math.Floor(rect..x + currentPosition), (float)(int)position.y, (float)num3, Math.Floor(cachedHeaderHeight));
                def.columns[i].Worker.DoHeader(rects, this);
                currentPosition += num3;
            }

            Rect outRect = rect.BottomPart(cachedHeaderHeight);
            Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, (float)Math.Floor(cachedHeightNoScrollbar - cachedHeaderHeight));

            // Draw Body
            Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect, true);

            int num4 = 0;
            foreach (var measurement in RimProfiler.EntityMeasurer.Measurements)
            {
                currentPosition = 0;
                if (!((float)num4 - scrollPosition.y + (float)(int)cachedRowHeights[j] < 0f) && !((float)num4 - scrollPosition.y > outRect.height))
                {
                    GUI.color = new Color(1f, 1f, 1f, 0.2f);
                    Widgets.DrawLineHorizontal(0f, (float)num4, viewRect.width);
                    GUI.color = Color.white;
                    Rect rect2 = new Rect(0f, (float)num4, viewRect.width, (float)(int)cachedRowHeights[j]);
                    if (Mouse.IsOver(rect2))
                    {
                        GUI.DrawTexture(rect2, TexUI.HighlightTex);
                    }
                    for (int k = 0; k < def.columns.Count; k++)
                    {
                        int num5 = (k != def.columns.Count - 1) ? ((int)cachedColumnWidths[k]) : ((int)(viewportWidth - (float)currentPosition));
                        Rect rect3 = new Rect((float)currentPosition, (float)num4, (float)num5, (float)(int)cachedRowHeights[j]);
                        def.columns[k].Worker.DoCell(rect3, cachedPawns[j], this);
                        currentPosition += num5;
                    }
                    if (cachedPawns[j].Downed)
                    {
                        GUI.color = new Color(1f, 0f, 0f, 0.5f);
                        Vector2 center = rect2.center;
                        Widgets.DrawLineHorizontal(0f, center.y, viewRect.width);
                        GUI.color = Color.white;
                    }
                }
                num4 += (int)cachedRowHeights[j];
            }
            Widgets.EndScrollView();
        }

        //private void RecacheSize()
        //{
        //    if (hasFixedSize)
        //    {
        //        cachedSize = fixedSize;
        //    }
        //    else
        //    {
        //        float num = 0f;
        //        for (int i = 0; i < def.columns.Count; i++)
        //        {
        //            if (!def.columns[i].ignoreWhenCalculatingOptimalTableSize)
        //            {
        //                num += GetOptimalWidth(def.columns[i]);
        //            }
        //        }
        //        float a = Mathf.Clamp(num + 16f, (float)minTableWidth, (float)maxTableWidth);
        //        float a2 = Mathf.Clamp(cachedHeightNoScrollbar, (float)minTableHeight, (float)maxTableHeight);
        //        a = Mathf.Min(a, (float)UI.screenWidth);
        //        a2 = Mathf.Min(a2, (float)UI.screenHeight);
        //        cachedSize = new Vector2(a, a2);
        //    }
        //}
    }

    class Column
    {

    }
}
