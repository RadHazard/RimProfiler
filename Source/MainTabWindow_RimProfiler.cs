using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

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



            var cachedHeightNoScrollbar = 3f;//TODO
            var cachedHeaderHeight = 3f;//TODO
            var cachedRowHeight = 3f; //TODO

            var columns = new List<Column>();

            // Draw Header
            float viewportWidth = size.x - 16f;
            int xPos = 0;
            foreach (var column in columns)
            {
                //TODO truncate width of last column
                //int columnWidth = (i != def.columns.Count - 1) ? ((int)cachedColumnWidths[i]) : ((int)(viewportWidth - (float)currentPosition));
                var columnWidth = column.Width;
                Rect headerRect = new Rect(rect.xMin + xPos, rect.yMin, columnWidth, cachedHeaderHeight);
                column.DoHeader(headerRect);
                xPos += columnWidth;
            }

            Rect outRect = rect.BottomPart(cachedHeaderHeight);
            Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, (float)Math.Floor(cachedHeightNoScrollbar - cachedHeaderHeight));

            // Draw Body
            Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect, true);

            // TODO - abstract this into a virtual scrolling class - UI toolkit?
            // Only render the rows that are in view
            int rowCountToSkip = (int) Math.Floor(scrollPosition.y / cachedRowHeight);
            int rowCountToRender = (int) Math.Ceiling(outRect.height / cachedRowHeight) + 1; // Add a an extra because we show a partial row at both ends

            // TODO - does C# have an enumerate() equivalent?
            var measurements = RimProfiler.EntityMeasurer.Measurements;
            for (int i = 0; i <= rowCountToRender; i++)
            {
                int currentRow = rowCountToSkip + i;
                var measurement = measurements[currentRow];
                float yPos = currentRow * cachedRowHeight; 

                // Draw each row
                GUI.color = new Color(1f, 1f, 1f, 0.2f);
                Widgets.DrawLineHorizontal(0f, yPos, viewRect.width);

                GUI.color = Color.white;
                Rect rowRect = new Rect(0f, yPos, viewRect.width, cachedRowHeight);
                if (Mouse.IsOver(rowRect))
                {
                    GUI.DrawTexture(rowRect, TexUI.HighlightTex);
                }

                xPos = 0;
                foreach (var column in columns)
                {
                    // Draw each cell
                    //TODO truncate width of last column
                    //int columnWidth = (i != def.columns.Count - 1) ? ((int)cachedColumnWidths[i]) : ((int)(viewportWidth - (float)currentPosition));
                    Rect cellRect = new Rect(xPos, yPos, column.Width, cachedRowHeight);
                    column.DoCell(cellRect, measurement);
                    xPos += column.Width;
                }

                yPos += cachedRowHeight;
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

    internal class Column
    {
        public int Width { get; private set; }

        internal Column(int width)
        {
            Width = width;
        }

        internal void DoHeader(Rect headerRect)
        {
            // TODO
            throw new NotImplementedException();
        }

        internal void DoCell(Rect cellRect, KeyValuePair<string, AverageResult> measurement)
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}
