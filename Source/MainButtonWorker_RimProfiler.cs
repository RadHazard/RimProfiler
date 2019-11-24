using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace RimProfiler
{
    public class MainButtonWorker_RimProfiler : MainButtonWorker_ToggleTab
    {

        //public override void DoButton(Rect rect)
        //{
        //    Text.Font = GameFont.Small;
        //    string buttonText = def.LabelCap;
        //    float buttonTextWidth = def.LabelCapWidth;
        //    if (buttonTextWidth > rect.width - 2f)
        //    {
        //        buttonText = def.ShortenedLabelCap;
        //        buttonTextWidth = def.ShortenedLabelCapWidth;
        //    }

        //    if (enableBar || enableTip || onScreenMemUsage)
        //    {
        //        updatetick++;
        //        if (updatetick > updateInterval)
        //        {
        //            updatetick = 0;

        //            long mem = GC.GetTotalMemory(false) / 1024;
        //            float memMb = mem / 1024f;
        //            if (enableTip)
        //            {
        //                tipCache = string.Format(MMTipTranslated, memMb);
        //            }
        //            if (enableBar)
        //            {
        //                progress = Mathf.Clamp01((memMb - memoryBarLowerMb) / memoryBarStepMb);
        //            }
        //            if (onScreenMemUsage)
        //            {
        //                labelCache = string.Format("{0:F2} Mb\n{1} Kb", memMb, mem);
        //            }
        //        }
        //    }

        //    bool flag = buttonTextWidth > 0.85f * rect.width - 1f;
        //    string label = onScreenMemUsage ? labelCache : buttonText;
        //    float textLeftMargin = (!flag) ? -1f : 2f;
        //    if (Widgets.ButtonTextSubtle(rect, label, progress, textLeftMargin, SoundDefOf.Mouseover_Category, default(Vector2)))
        //    {
        //        if (Current.ProgramState == ProgramState.Playing)
        //            InterfaceTryActivate();
        //    }

        //    TooltipHandler.TipRegion(rect, TabDescriptionTranslated + tipCache);
        //}
    }
}
