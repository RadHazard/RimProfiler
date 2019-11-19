using Harmony;
using Verse;

namespace RimProfiler.Patch
{
    [HarmonyPatch(typeof(TickList), "Tick")]
    class TickListTick
    {
        static void Prefix(TickerType ___tickType)
        {
            RimProfiler.TickProfiler.TickListStart(___tickType);
        }
        static void Postfix(TickerType ___tickType)
        {
            RimProfiler.TickProfiler.TickListEnd(___tickType);
        }

    }
}
