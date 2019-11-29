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
                    .BottomPart(size.y - ExtraTopSpace)
                    .TopPart(size.y - (ExtraBottomSpace)); // TODO finish this

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

            Log.Message(string.Format("({0}, {1}) ({2}, {3})", rect.xMin, rect.xMax, rect.yMin, rect.yMax));

            //Listing_Standard listing = new Listing_Standard();
            //listing.Begin(rect);

            //foreach (var measurement in RimProfiler.EntityMeasurer.Measurements)
            //{
            //    listing.Label(string.Format("{0,-16}  {1,10:N2}    {2}", measurement.Duration, measurement.Invocations, measurement.Name));
            //}

            //listing.End();
            
            var cachedHeightNoScrollbar = 30f;//TODO
            var cachedHeaderHeight = 30f;//TODO
            var cachedRowHeight = 30f; //TODO

            var columns = new List<Column>
            {
                new NameColumn(),
                new DurationColumn(),
                new InvocationsColumn()
            };

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

            var measurements = RimProfiler.EntityMeasurer.Measurements;

            // TODO - abstract this into a virtual scrolling class - UI toolkit?
            // Only render the rows that are in view
            int rowCountToSkip = (int)Math.Floor(scrollPosition.y / cachedRowHeight);
            int rowCountToRender = (int)Math.Min((Math.Ceiling(outRect.height / cachedRowHeight) + 1), measurements.Count - rowCountToSkip); // Add a an extra because we show a partial row at both ends

            // TODO - does C# have an enumerate() equivalent?
            for (int i = 0; i < rowCountToRender; i++)
            {
                int currentRow = rowCountToSkip + i;
                if (currentRow >= measurements.Count)
                {
                    Log.Error("Out of range! " + currentRow + "/" + measurements.Count);
                    break;
                }

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

            GUI.EndGroup();
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

    internal abstract class Column
    {
        public string Label { get; }
        public int Width { get; }

        protected virtual Color DefaultHeaderColor => Color.white;
        protected virtual GameFont DefaultHeaderFont => GameFont.Small;

        internal Column(string label, int width)
        {
            Label = label;
            Width = width;
        }

        internal void DoHeader(Rect headerRect)
        {
            if (!Label.NullOrEmpty())
            {
                Text.Font = DefaultHeaderFont;
                GUI.color = DefaultHeaderColor;
                Text.Anchor = TextAnchor.LowerCenter;

                Widgets.Label(headerRect.BottomPart(3f), Label.Truncate(headerRect.width, null));

                Text.Anchor = TextAnchor.UpperLeft;
                GUI.color = Color.white;
                Text.Font = GameFont.Small;
            }
        }

        internal void DoCell(Rect cellRect, AverageResult measurement)
        {
            Rect rect2 = cellRect.BottomPart(30f);
            string textFor = GetText(measurement);
            if (!textFor.NullOrEmpty())
            {
                Text.Font = GameFont.Small;
                Text.Anchor = TextAnchor.MiddleLeft;
                Text.WordWrap = false;

                Widgets.Label(rect2, textFor);

                Text.WordWrap = true;
                Text.Anchor = TextAnchor.UpperLeft;
            }
        }

        protected abstract string GetText(AverageResult measurement);
    }

    internal class NameColumn : Column
    {
        internal NameColumn() : base("Name", 250) { }

        protected override string GetText(AverageResult measurement)
        {
            return measurement.Name;
        }
    }

    internal class DurationColumn : Column
    {
        internal DurationColumn() : base("Duration", 75) { }

        protected override string GetText(AverageResult measurement)
        {
            return measurement.Duration.ToStringSafe();
        }
    }

    internal class InvocationsColumn : Column
    {
        internal InvocationsColumn() : base("Invocations", 25) { }

        protected override string GetText(AverageResult measurement)
        {
            return measurement.Invocations.ToStringSafe();
        }
    }

    //public abstract class PawnColumnWorker_Text : PawnColumnWorker
    //{
    //    private static NumericStringComparer comparer = new NumericStringComparer();

    //    protected virtual int Width => def.width;

    //    public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
    //    {
    //        Rect rect2 = new Rect(rect.x, rect.y, rect.width, Mathf.Min(rect.height, 30f));
    //        string textFor = GetTextFor(pawn);
    //        if (textFor != null)
    //        {
    //            Text.Font = GameFont.Small;
    //            Text.Anchor = TextAnchor.MiddleLeft;
    //            Text.WordWrap = false;
    //            Widgets.Label(rect2, textFor);
    //            Text.WordWrap = true;
    //            Text.Anchor = TextAnchor.UpperLeft;
    //            string tip = GetTip(pawn);
    //            if (!tip.NullOrEmpty())
    //            {
    //                TooltipHandler.TipRegion(rect2, tip);
    //            }
    //        }
    //    }

    //    public override int GetMinWidth(PawnTable table)
    //    {
    //        return Mathf.Max(base.GetMinWidth(table), Width);
    //    }

    //    public override int Compare(Pawn a, Pawn b)
    //    {
    //        return comparer.Compare(GetTextFor(a), GetTextFor(a));
    //    }

    //    protected abstract string GetTextFor(Pawn pawn);

    //    protected virtual string GetTip(Pawn pawn)
    //    {
    //        return null;
    //    }

    //    public virtual void DoHeader(Rect rect, PawnTable table)
    //    {

    //    }
    //}
}
