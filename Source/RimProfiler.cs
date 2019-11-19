using System.Diagnostics;
using System.Reflection;
using Harmony;
using Verse;

namespace RimProfiler
{
    [StaticConstructorOnStartup]
    public static class RimProfiler
    {
        public static readonly TickProfiler TickProfiler = new TickProfiler();

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
                Log.Message("Operations timed using the system's high-resolution performance counter.");
            }
            else
            {
                Log.Message("Operations timed using the DateTime class.");
            }

            long frequency = Stopwatch.Frequency;
            Log.Message("  Timer frequency in ticks per second = {0}".Formatted(frequency));
            long nanosecPerTick = (1000L * 1000L * 1000L) / frequency;
            Log.Message("  Timer is accurate within {0} nanoseconds".Formatted(nanosecPerTick));
        }
    }
}
