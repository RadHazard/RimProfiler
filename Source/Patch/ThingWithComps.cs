using Harmony;
using Verse;

namespace RimProfiler.Patch
{
    [HarmonyPatch(typeof(ThingWithComps), "Tick")]
    class ThingWithCompsTick
    {
        static void Prefix(ThingWithComps __instance)
        {
            RimProfiler.EntityMeasurer.EntityTickStart(EntityIdGetter.GetId(__instance, "tick"));
        }

        static void Postfix(ThingWithComps __instance)
        {
            RimProfiler.EntityMeasurer.EntityTickEnd(EntityIdGetter.GetId(__instance, "tick"));
        }
    }

    [HarmonyPatch(typeof(ThingWithComps), "TickRare")]
    class ThingWithCompsTickRare
    {
        static void Prefix(ThingWithComps __instance)
        {
            RimProfiler.EntityMeasurer.EntityTickStart(EntityIdGetter.GetId(__instance, "tickRare"));
        }

        static void Postfix(ThingWithComps __instance)
        {
            RimProfiler.EntityMeasurer.EntityTickEnd(EntityIdGetter.GetId(__instance, "tickRare"));
        }
    }

    [HarmonyPatch(typeof(ThingWithComps), "TickLong")]
    class ThingWithCompsTickLong
    {
        static void Prefix(ThingWithComps __instance)
        {
            RimProfiler.EntityMeasurer.EntityTickStart(EntityIdGetter.GetId(__instance, "tickLong"));
        }

        static void Postfix(ThingWithComps __instance)
        {
            RimProfiler.EntityMeasurer.EntityTickEnd(EntityIdGetter.GetId(__instance, "tickLong"));
        }
    }
}
