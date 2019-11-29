using System;
using System.Diagnostics;

namespace RimProfiler
{
    /// <summary>
    /// A profiler that keeps a running total of how long an operation takes
    /// and stores the history for that
    /// </summary>
    public class Profiler
    {
        private readonly Stopwatch stopwatch = new Stopwatch();
        private int invocations;

        public ProfilerHistory History { get; }

        public Profiler(string name)
        {
            History = new ProfilerHistory(name, RimProfiler.MaxHistoryEntries);
        }

        /// <summary>
        /// Starts or resumes the profiler's measurement
        /// </summary>
        public void Start()
        {
            invocations++;
            stopwatch.Start();
        }

        /// <summary>
        /// Pauses the profiler's measurement but doesn't reset it
        /// </summary>
        public void Pause()
        {
            stopwatch.Stop();
        }

        /// <summary>
        /// Records the total measured time since the profiler was last reset,
        /// then resets the profiler.
        /// </summary>
        /// <returns>The total time recorded by this profiler</returns>
        public void RecordMeasurement()
        {
            History.AddMeasurement(stopwatch.Elapsed, invocations);
            invocations = 0;
            stopwatch.Reset();
        }
    }
}
