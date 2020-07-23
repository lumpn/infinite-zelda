using System;
using Lumpn.Mooga;
using Lumpn.Dungeon;

namespace Lumpn.ZeldaMooga
{
    public sealed class ZeldaIndividual : Individual
    {
        private readonly ZeldaGenome genome;

        private readonly Crawler crawler;

        // statistics
        private readonly int genomeSize;
        private readonly int genomeErrors;
        private readonly int numErrors;
        private readonly int shortestPathLength;
        private readonly double revisitFactor;
        private readonly double branchFactor;

        private static int Minimize(int value)
        {
            return -value;
        }

        private static int Maximize(int value)
        {
            return value;
        }

        private static int Target(int value, int target)
        {
            return Maximize(target - Math.Abs(value - target));
        }

        private static double Maximize(double value)
        {
            return value;
        }

        private static int Prefer(bool value)
        {
            return value ? 1 : 0;
        }

        public ZeldaIndividual(ZeldaGenome genome, ZeldaPuzzle puzzle, int numErrors, int shortestPathLength, double revisitFactor, double branchFactor)
        {
            assert genome != null;
            assert puzzle != null;
            this.genome = genome;
            this.puzzle = puzzle;
            this.genomeSize = genome.size();
            this.genomeErrors = genome.countErrors();
            this.numErrors = numErrors;
            this.shortestPathLength = shortestPathLength;
            this.revisitFactor = revisitFactor;
            this.branchFactor = branchFactor;
        }

        public Genome getGenome()
        {
            return genome;
        }

        public int numAttributes()
        {
            return 10;
        }

        public int getPriority(int attribute)
        {
            switch (attribute)
            {
                case 0:
                    return 10;
                case 1:
                    return 50;
                case 2:
                    return 50;
                case 3:
                    return 100;
                case 4:
                    return 70;
                case 5:
                    return 30;
                case 6:
                    return 30;
                case 7:
                    return 30;
                case 8:
                    return 90;
                case 9:
                    return 90;
                default:
                    assert false;
            }
            return 0;
        }

        public double getScore(int attribute)
        {
            switch (attribute)
            {
                case 0:
                    return Minimize(genomeSize);
                case 1:
                    return Minimize(genomeErrors);
                case 2:
                    return Minimize(numErrors);
                case 3:
                    return Prefer(shortestPathLength != Step.UNREACHABLE);
                case 4:
                    return Prefer(shortestPathLength > 10);
                case 5:
                    return Maximize(revisitFactor);
                case 6:
                    return Maximize(branchFactor);
                case 7:
                    return Maximize(shortestPathLength);
                case 8:
                    return Prefer(genomeErrors < 10);
                case 9:
                    return Prefer(numErrors < 10);
                default:
                    assert false;// TODO: minimize unused transitions
            }
            return 0;
        }


        public String toString()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < numAttributes(); i++)
            {
                builder.append(getScore(i));
                builder.append(" ");
            }
            builder.append(String.format("(%d)", puzzle.getSteps().size()));
            return String.format("%s: %s", builder.toString(), genome);
        }

        public ZeldaPuzzle puzzle()
        {
            return puzzle;
        }
    }
}
