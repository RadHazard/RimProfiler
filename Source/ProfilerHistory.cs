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
        private readonly Queue<HistoryEntry> q = new Queue<HistoryEntry>();
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
        /// <param name="invocations">The number of separate invocations</param>
        public void AddMeasurement(TimeSpan measurement, int invocations)
        {

            q.Enqueue(new HistoryEntry(measurement, invocations));

            // Remove old entries if we're over the limit
            while (q.Count > MaxEntries) q.Dequeue();
        }

        /// <summary>
        /// Gets the last <paramref name="count"/> measurements, newest to oldest
        /// </summary>
        /// <returns>An IEnumerable of the latest run time measurements.</returns>
        /// <param name="count">The number of runtimes to return.</param>
        public IEnumerable<HistoryEntry> GetLatestMeasurements(int count)
        {
            return q.Reverse().Take(count);
        }

        /// <summary>
        /// Returns the average of up to the last <paramref name="count"/> run
        /// time measurements.
        /// </summary>
        /// <returns>The average runtime.</returns>
        /// <param name="count">The number of runtimes to average.</param>
        public AverageResult AverageHistory(int count)
        {
            var measurements = GetLatestMeasurements(count);
            long averageTicks = Convert.ToInt64(measurements.Average(i => i.Duration.Ticks));
            double averageInvocations = measurements.Average(i => i.Invocations);

            return new AverageResult(new TimeSpan(averageTicks), averageInvocations);
        }
    }

    public struct HistoryEntry
    {
        public TimeSpan Duration { get; private set; }
        public int Invocations { get; private set; }

        public HistoryEntry(TimeSpan duration, int invocations)
        {
            Duration = duration;
            Invocations = invocations;
        }
    }

    public struct AverageResult
    {
        public TimeSpan Duration { get; private set; }
        public double Invocations { get; private set; }

        public AverageResult(TimeSpan duration, double invocations)
        {
            Duration = duration;
            Invocations = invocations;
        }
    }
}
