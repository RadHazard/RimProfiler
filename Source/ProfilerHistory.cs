using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace RimProfiler
{
    /// <summary>
    /// A running history of performance runtimes.  Recordes measurements up
    /// to the limit and has multiple operations to get statistics on the runtimes
    /// </summary>
    public class ProfilerHistory
    {

        private readonly Stopwatch stopwatch = new Stopwatch();
        private readonly Queue<TimeSpan> q = new Queue<TimeSpan>();
        private readonly long MaxEntries;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RimProfiler.Profiler"/> class.
        /// </summary>
        /// <param name="maxEntries">The maximum number of entries to keep in history</param>
        public ProfilerHistory(int maxEntries)
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
        /// Records the given <paramref name="measurement"/> as a history entry
        /// </summary>
        /// <param name="measurement">The measurement to record.</param>
        public void AddMeasurement(TimeSpan measurement)
        {
            q.Enqueue(measurement);

            // Remove old entries if we're over the limit
            while (q.Count > MaxEntries) q.Dequeue();
        }

        /// <summary>
        /// Gets the last <paramref name="count"/> measurements, newest to oldest
        /// </summary>
        /// <returns>An IEnumerable of the latest run time measurements.</returns>
        /// <param name="count">The number of runtimes to return.</param>
        public IEnumerable<TimeSpan> GetLatestMeasurements(int count)
        {
            return q.Reverse().Take(count);
        }

        /// <summary>
        /// Returns the average of up to the last <paramref name="count"/> run
        /// time measurements.
        /// </summary>
        /// <returns>The average runtime.</returns>
        /// <param name="count">The number of runtimes to average.</param>
        public TimeSpan GetAverageTime(int count)
        {
            double doubleAverageTicks = GetLatestMeasurements(count).Average(timeSpan => timeSpan.Ticks);
            long longAverageTicks = Convert.ToInt64(doubleAverageTicks);

            return new TimeSpan(longAverageTicks);
        }
    }
}
