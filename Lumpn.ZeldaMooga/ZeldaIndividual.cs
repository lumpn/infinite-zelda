using System.Text;
using Lumpn.Mooga;
using Lumpn.Utils;
using Lumpn.Dungeon;

namespace Lumpn.ZeldaMooga
{
    public sealed class ZeldaIndividual : Individual
    {
        // configuration
        public const int NumAttributes = 5;

        // statistics
        private readonly int numSteps;
        private readonly int numDeadEnds;
        private readonly int shortestPathLength;
        private readonly double revisitFactor;
        private readonly double branchFactor;

        private readonly ZeldaGenome genome;
        private readonly Crawler crawler;

        public Genome Genome { get { return genome; } }
        public Crawler Crawler {  get { return crawler; } }

        public ZeldaIndividual(ZeldaGenome genome, Crawler crawler, int numSteps, int numDeadEnds, int shortestPathLength, double revisitFactor, double branchFactor)
        {
            Debug.Assert(genome != null);
            this.genome = genome;
            this.crawler = crawler;
            this.numSteps = numSteps;
            this.numDeadEnds = numDeadEnds;
            this.shortestPathLength = shortestPathLength;
            this.revisitFactor = revisitFactor;
            this.branchFactor = branchFactor;
        }

        public double GetScore()
        {
            return GetScore(0)
                 + GetScore(1)
                 + GetScore(2)
                 + GetScore(3)
                 + GetScore(4);
        }

        public double GetScore(int attribute)
        {
            switch (attribute)
            {
                case 0: return OptimizationUtils.Maximize(numSteps);
                case 1: return OptimizationUtils.Minimize(numDeadEnds);
                case 2: return OptimizationUtils.Maximize(revisitFactor);
                case 3: return OptimizationUtils.Maximize(branchFactor);
                case 4: return OptimizationUtils.Maximize(shortestPathLength);
            }

            Debug.Fail();
            return 0;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            for (int i = 0; i < NumAttributes; i++)
            {
                builder.Append(GetScore(i));
                builder.Append(", ");
            }
            builder.Append("(");
            builder.Append(numSteps);
            builder.Append("): ");
            builder.Append(genome);

            return builder.ToString();
        }
    }
}
