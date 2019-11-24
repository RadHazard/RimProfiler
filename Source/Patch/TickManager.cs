using Harmony;
using Verse;

namespace RimProfiler.Patch
{
    [HarmonyPatch(typeof(TickManager), "DoSingleTick")]
    class TickManagerDoSingleTick
    {
        static void Prefix()
        {
            RimProfiler.EntityMeasurer.TickStart();
        }
        static void Postfix()
        {
            RimProfiler.EntityMeasurer.TickEnd();
        }

    }
}
