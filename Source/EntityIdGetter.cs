using Verse;

namespace RimProfiler
{
    public static class EntityIdGetter
    {
        public static string GetId(Entity entity, string suffex)
        {
            return "{0}_{1}".Formatted(entity.GetType().ToString(), suffex);
        }
    }
}
