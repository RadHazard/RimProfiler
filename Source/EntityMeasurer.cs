using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimProfiler
{
    public class EntityMeasurer
    {
        private readonly Dictionary<string, Profiler> entityProfilerDictionary = new Dictionary<string, Profiler>();
        private bool profile;
        private SortBy sortingMethod = SortBy.DURATION; //TODO - add the ability to switch this

        public List<KeyValuePair<string, AverageResult>> Measurements { get; private set; } = new List<KeyValuePair<string, AverageResult>>();


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
                    Func<KeyValuePair<string, AverageResult>, dynamic> orderingFunction;

                    switch (sortingMethod)
                    {
                        case SortBy.DURATION:
                            orderingFunction = i => i.Value.Duration;
                            break;
                        case SortBy.INVOCATIONS:
                            orderingFunction = i => i.Value.Invocations;
                            break;
                        default:
                            throw new NotImplementedException("Invalid sorting method");
                    }


                    Measurements = entityProfilerDictionary.Select(i =>
                    {
                        var value = i.Value.History.AverageHistory(RimProfiler.AveragingTime);
                        return new KeyValuePair<string, AverageResult>(i.Key, value);
                    })
                    .OrderByDescending(orderingFunction)
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

        public enum SortBy
        {
            DURATION,
            INVOCATIONS
        }
    }
}
