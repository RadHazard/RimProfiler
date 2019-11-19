using Verse;

namespace RimProfiler
{
    public class TickProfiler
    {
        private static readonly int HistorySize = 3600;
        private static readonly int AverageOverTicks = 600;

        private readonly Profiler tickProfiler = new Profiler(HistorySize);
        private readonly Profiler tickerNormalProfiler = new Profiler(HistorySize);
        private readonly Profiler tickerRareProfiler = new Profiler(HistorySize);
        private readonly Profiler tickerLongProfiler = new Profiler(HistorySize);

        public void TickStart()
        {
            tickProfiler.Start();
        }

        public void TickEnd()
        {
            tickProfiler.End();

            if (GenTicks.TicksGame % AverageOverTicks == 0)
            {
                Log.Message("Average tick time in the last 600 ticks (~10 seconds): {0}\nNormal Ticker: {1}\nLong Ticker:   {2}\nRare Ticker:   {3}".Formatted(
                        tickProfiler.GetAverageTime(AverageOverTicks).ToString(),
                        tickerNormalProfiler.GetAverageTime(AverageOverTicks).ToString(),
                        tickerRareProfiler.GetAverageTime(AverageOverTicks).ToString(),
                        tickerLongProfiler.GetAverageTime(AverageOverTicks).ToString()
                ));
                //Log.Message("Last 10: {0}"
                //        .Formatted(string.Join(",", tickTime.GetLatestTimes(10).Select(i => i.ToString()).ToArray())));
            }
        }

        public void TickListStart(TickerType type)
        {
            switch (type)
            {
                case TickerType.Normal:
                    tickerNormalProfiler.Start();
                    break;
                case TickerType.Rare:
                    tickerRareProfiler.Start();
                    break;
                case TickerType.Long:
                    tickerLongProfiler.Start();
                    break;
            }
        }

        public void TickListEnd(TickerType type)
        {
            switch (type)
            {
                case TickerType.Normal:
                    tickerNormalProfiler.End();
                    break;
                case TickerType.Rare:
                    tickerRareProfiler.End();
                    break;
                case TickerType.Long:
                    tickerLongProfiler.End();
                    break;
            }
        }
    }
}
