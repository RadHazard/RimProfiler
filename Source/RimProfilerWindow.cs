using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;

namespace RimProfiler
{
    public class RimProfilerWindow : MainTabWindow
    {
        public override Vector2 RequestedTabSize => new Vector2(350f, 500f);

        public override void DoWindowContents(Rect inRect)
        {
            GUI.BeginGroup(inRect);

            // TODO

            GUI.EndGroup();
        }
    }
}
