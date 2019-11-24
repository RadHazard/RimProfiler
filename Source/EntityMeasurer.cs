using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimProfiler
{
    public class EntityMeasurer
    {
        private readonly Dictionary<string, Profiler> entityProfilerDictionary = new Dictionary<string, Profiler>();

        public void TickStart()
        {

        }

        public void TickEnd()
        {
            foreach (Profiler profiler in entityProfilerDictionary.Values)
            {
                profiler.RecordMeasurement();
            }

            if (GenTicks.TicksGame % RimProfiler.AverageOverTicks == 0)
            {
                var topTen = GetTopX(10).Select(i => string.Format("{0}    {1}", i.Value, i.Key));
                Log.Message(string.Format("Top ten tickers:\n{0}", string.Join("\n", topTen.ToArray())));
                Log.Message(string.Format("Total Entities: {0}", entityProfilerDictionary.Count));
            }
        }

        public void EntityTickStart(string entityId)
        {
            if (!entityProfilerDictionary.ContainsKey(entityId))
            {
                entityProfilerDictionary[entityId] = new Profiler();
            }

            entityProfilerDictionary[entityId].Start();
        }

        public void EntityTickEnd(string entityId)
        {
            entityProfilerDictionary[entityId].Pause();
        }

        public IEnumerable<KeyValuePair<string, System.TimeSpan>> GetTopX(int count)
        {
            return entityProfilerDictionary.Select(i =>
                        {
                            var value = i.Value.History.GetAverageTime(RimProfiler.AverageOverTicks);
                            return new KeyValuePair<string, System.TimeSpan>(i.Key, value);
                        })
                    .OrderByDescending(i => i.Value)
                    .Take(count);
        }
    }
}
