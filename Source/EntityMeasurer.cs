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

        public List<AverageResult> Measurements { get; private set; } = new List<AverageResult>();


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
                    Func<AverageResult, dynamic> orderingFunction;

                    switch (sortingMethod)
                    {
                        case SortBy.DURATION:
                            orderingFunction = i => i.Duration;
                            break;
                        case SortBy.INVOCATIONS:
                            orderingFunction = i => i.Invocations;
                            break;
                        default:
                            throw new NotImplementedException("Invalid sorting method");
                    }


                    Measurements = entityProfilerDictionary.Values.Select(i =>
                    {
                        return i.History.AverageHistory(RimProfiler.AveragingTime);
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
                    entityProfilerDictionary[entityId] = new Profiler(entityId);
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
