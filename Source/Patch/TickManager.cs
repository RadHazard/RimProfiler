using Harmony;
using Verse;

namespace RimProfiler.Patch
{
    [HarmonyPatch(typeof(TickManager), "DoSingleTick")]
    class TickManagerDoSingleTick
    {
        static void Prefix()
        {
            RimProfiler.TickProfiler.TickStart();
        }
        static void Postfix()
        {
            RimProfiler.TickProfiler.TickEnd();
        }

    }
}
