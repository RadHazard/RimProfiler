using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimProfiler
{
    public class EntityMeasurer
    {
        private readonly Dictionary<string, Profiler> entityProfilerDictionary = new Dictionary<string, Profiler>();
        public List<KeyValuePair<string, TimeSpan>> Measurements { get; private set; } = new List<KeyValuePair<string, TimeSpan>>();

        private bool profile;


        public void StartProfiling()
        {
            if (!profile)
            {
                profile = true;
                Log.Message("Starting profiling");
            }
        }

        public void StopProfiling()
        {
            if (profile)
            {
                profile = false;
                Log.Message("Pausing profiling");
            }
        }

        public void TickStart()
        {

        }

        public void TickEnd()
        {
            if (profile)
            {
                foreach (Profiler profiler in entityProfilerDictionary.Values)
                {
                    profiler.RecordMeasurement();
                }

                if (GenTicks.TicksGame % RimProfiler.UpdateInterval == 0)
                {
                    Measurements = entityProfilerDictionary.Select(i =>
                    {
                        var value = i.Value.History.GetAverageTime(RimProfiler.AveragingTime);
                        return new KeyValuePair<string, TimeSpan>(i.Key, value);
                    })
                    .OrderByDescending(i => i.Value)
                    .ToList();
                }
            }
        }

        public void EntityTickStart(string entityId)
        {
            if (profile)
            {

                if (!entityProfilerDictionary.ContainsKey(entityId))
                {
                    entityProfilerDictionary[entityId] = new Profiler();
                }

                entityProfilerDictionary[entityId].Start();
            }
        }

        public void EntityTickEnd(string entityId)
        {
            if (profile)
            {
                entityProfilerDictionary[entityId].Pause();
            }
        }
    }
}
