using System.Text;
using Lumpn.Mooga;
using Lumpn.Utils;
using Lumpn.Dungeon;
using Lumpn.Dungeon.Scripts;

namespace Lumpn.ZeldaMooga
{
    public sealed class ZeldaIndividual : Individual
    {
        // configuration
        public const int NumAttributes = 3;

        // statistics
        private readonly int shortestPathLength;
        private readonly int numDeadEnds;
        private readonly int numTerminalStates;

        private readonly ZeldaGenome genome;
        private readonly CrawlerBuilder crawler;

        public Genome Genome { get { return genome; } }

        public ZeldaIndividual(ZeldaGenome genome, CrawlerBuilder crawler, Trace trace)
        {
            Debug.Assert(genome != null);
            this.genome = genome;
            this.crawler = crawler;
            this.shortestPathLength = trace.CalcShortestPathLength(0);
            this.numDeadEnds = trace.CountDeadEnds();
            this.numTerminalStates = Replace(0, 100, trace.CountSteps(0));
        }

        public double GetScore(int attribute)
        {
            switch (attribute)
            {
                case 0: return OptimizationUtils.Maximize(shortestPathLength);
                case 1: return OptimizationUtils.Minimize(numDeadEnds);
                case 2: return OptimizationUtils.Minimize(numTerminalStates);
            }

            Debug.Fail();
            return 0;
        }

        public void Express(DotBuilder builder)
        {
            crawler.Express(builder, NoOpScript.instance);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(GetScore(0));
            for (int i = 1; i < NumAttributes; i++)
            {
                builder.Append(", ");
                builder.Append(GetScore(i));
            }
            builder.Append(": ");
            builder.Append(genome);

            return builder.ToString();
        }

        private static int Replace(int before, int after, int value)
        {
            return (value == before) ? after : value;
        }
    }
}
