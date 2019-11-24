using Harmony;
using Verse;

namespace RimProfiler.Patch
{
    [HarmonyPatch(typeof(Entity), "Tick")]
    class EntityTick
    {
        static void Prefix(Entity __instance)
        {
            RimProfiler.EntityMeasurer.EntityTickStart(EntityIdGetter.GetId(__instance, "tick"));
        }

        static void Postfix(Entity __instance)
        {
            RimProfiler.EntityMeasurer.EntityTickEnd(EntityIdGetter.GetId(__instance, "tick"));
        }
    }

    [HarmonyPatch(typeof(Entity), "TickRare")]
    class EntityTickRare
    {
        static void Prefix(Entity __instance)
        {
            RimProfiler.EntityMeasurer.EntityTickStart(EntityIdGetter.GetId(__instance, "tickRare"));
        }

        static void Postfix(Entity __instance)
        {
            RimProfiler.EntityMeasurer.EntityTickEnd(EntityIdGetter.GetId(__instance, "tickRare"));
        }
    }

    [HarmonyPatch(typeof(Entity), "TickLong")]
    class EntityTickLong
    {
        static void Prefix(Entity __instance)
        {
            RimProfiler.EntityMeasurer.EntityTickStart(EntityIdGetter.GetId(__instance, "tickLong"));
        }

        static void Postfix(Entity __instance)
        {
            RimProfiler.EntityMeasurer.EntityTickEnd(EntityIdGetter.GetId(__instance, "tickLong"));
        }
    }
}
