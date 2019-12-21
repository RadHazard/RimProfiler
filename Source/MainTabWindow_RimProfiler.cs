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

        private readonly Vector2 size = new Vector2(1000f, 500f); // TODO - calculate this
        private readonly GUITable<AverageResult> table =
                new GUITable<AverageResult>(
                        () => RimProfiler.EntityMeasurer.Measurements,
                        new NameColumn(),
                        new DurationColumn(),
                        new InvocationsColumn()
                );

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
            table.Render(inRect.ContractedBy(Margin)
                    .BottomPartPixels(size.y - ExtraTopSpace)
                    .TopPartPixels(size.y - ExtraBottomSpace)); // TODO finish this

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

    internal class NameColumn : Column<AverageResult>
    {
        internal NameColumn() : base("Name", 400) { }

        protected override string GetText(AverageResult datum)
        {
            return datum.Name;
        }
    }

    internal class DurationColumn : Column<AverageResult>
    {
        internal DurationColumn() : base("Duration", 150) { }

        protected override string GetText(AverageResult datum)
        {
            return datum.Duration.ToStringSafe();
        }
    }

    internal class InvocationsColumn : Column<AverageResult>
    {
        internal InvocationsColumn() : base("Invocations", 75) { }

        protected override string GetText(AverageResult datum)
        {
            return datum.Invocations.ToStringSafe();
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
