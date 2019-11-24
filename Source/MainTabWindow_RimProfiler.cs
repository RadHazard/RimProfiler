using RimWorld;
using UnityEngine;

namespace RimProfiler
{
    public class MainTabWindow_RimProfiler : MainTabWindow
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
