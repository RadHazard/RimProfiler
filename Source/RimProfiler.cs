using System.Diagnostics;
using System.Reflection;
using Harmony;
using Verse;

namespace RimProfiler
{
    [StaticConstructorOnStartup]
    public static class RimProfiler
    {
        public static readonly int MaxHistoryEntries = 3600;
        public static readonly int AverageOverTicks = 600;

        public static readonly EntityMeasurer EntityMeasurer = new EntityMeasurer();

        static RimProfiler()
        {
            var harmony = HarmonyInstance.Create("com.github.pausbrak.rimprofiler");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            DisplayTimerProperties();
        }

        public static void DisplayTimerProperties()
        {
            // Display the timer frequency and resolution.
            if (Stopwatch.IsHighResolution)
            {
                Log.Message("[RimProfiler] Stopwatch using the system's high-resolution performance counter.");
            }
            else
            {
                Log.Message("[RimProfiler] Stopwatch using the DateTime class.");
            }

            long frequency = Stopwatch.Frequency;
            Log.Message("  Timer frequency in ticks per second = {0}".Formatted(frequency));
            long nanosecPerTick = (1000L * 1000L * 1000L) / frequency;
            Log.Message("  Timer is accurate within {0} nanoseconds".Formatted(nanosecPerTick));
        }
    }
}
