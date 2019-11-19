using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace RimProfiler
{
    /// <summary>
    /// A profiler that measures the performance of a single block of code and
    /// keeps a running history of past performance.  Call Start() at the
    /// beginning of the code block you're measuring and End() at the end of it
    /// </summary>
    public class Profiler
    {

        private readonly Stopwatch stopwatch = new Stopwatch();
        private readonly Queue<TimeSpan> q = new Queue<TimeSpan>();
        private readonly long MaxEntries;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RimProfiler.Profiler"/> class.
        /// </summary>
        /// <param name="maxEntries">The maximum number of entries to keep in history</param>
        public Profiler(int maxEntries)
        {
            MaxEntries = maxEntries;
        }

        /// <summary>
        /// Starts the timer
        /// </summary>
        public void Start()
        {
            stopwatch.Reset();
            stopwatch.Start();
        }

        /// <summary>
        /// Ends the timer and records the runtime as a history entry
        /// </summary>
        public void End()
        {
            stopwatch.Stop();
            q.Enqueue(stopwatch.Elapsed);

            while (q.Count > MaxEntries) q.Dequeue();
        }

        /// <summary>
        /// Gets the last <paramref name="count"/> runtimes, newest to oldest
        /// </summary>
        /// <returns>The latest run times.</returns>
        /// <param name="count">The number of runtimes to return.</param>
        public IEnumerable<TimeSpan> GetLatestTimes(int count)
        {
            return q.Reverse().Take(count);
        }

        /// <summary>
        /// Returns the average of up to the last <paramref name="count"/> run
        /// times
        /// </summary>
        /// <returns>The average time.</returns>
        /// <param name="count">The number of runtimes to average.</param>
        public TimeSpan GetAverageTime(int count)
        {
            double doubleAverageTicks = GetLatestTimes(count).Average(timeSpan => timeSpan.Ticks);
            long longAverageTicks = Convert.ToInt64(doubleAverageTicks);

            return new TimeSpan(longAverageTicks);
        }
    }
}
