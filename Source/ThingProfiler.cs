using System;
using System.Diagnostics;

namespace RimProfiler
{
    /// <summary>
    /// A profiler that keeps a running total of how long an operation is
    /// </summary>
    public class ThingProfiler
    {

        private readonly Stopwatch stopwatch = new Stopwatch();
        
        /// <summary>
        /// Starts or resumes the profiler
        /// </summary>
        public void Start()
        {
            stopwatch.Start();
        }

        /// <summary>
        /// Pauses the profiler but doesn't reset it
        /// </summary>
        public void End()
        {
            stopwatch.Stop();
        }

        /// <summary>
        /// Records the total measured time since the profiler was last reset,
        /// then resets the profiler.
        /// </summary>
        /// <returns>The total time recorded by this profiler</returns>
        public TimeSpan RecordTime()
        {
            TimeSpan elapsed = stopwatch.Elapsed;
            stopwatch.Reset();
            return elapsed;
        }
    }
}
