using System.Text;
using Lumpn.Mooga;
using Lumpn.Utils;

namespace Lumpn.ZeldaMooga
{
    public sealed class ZeldaIndividual : Individual
    {
        // configuration
        private const int numAttributes = 5;

        // statistics
        private readonly int numSteps;
        private readonly int numDeadEnds;
        private readonly int shortestPathLength;
        private readonly double revisitFactor;
        private readonly double branchFactor;

        private readonly ZeldaGenome genome;

        public Genome Genome { get { return genome; } }

        public ZeldaIndividual(ZeldaGenome genome, int numSteps, int numDeadEnds, int shortestPathLength, double revisitFactor, double branchFactor)
        {
            Debug.Assert(genome != null);
            this.genome = genome;
            this.numDeadEnds = numDeadEnds;
            this.shortestPathLength = shortestPathLength;
            this.revisitFactor = revisitFactor;
            this.branchFactor = branchFactor;
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
            for (int i = 0; i < numAttributes; i++)
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
