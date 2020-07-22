using Lumpn.Utils;

namespace Lumpn.Mooga.Test
{
    public sealed class SimpleIndividual : Individual
    {
        public SimpleIndividual(double score)
        {
            this.score = score;
        }

        public Genome Genome { get { return null; } } // not needed for test

        public double GetScore(int attribute)
        {
            return score;
        }

        public override string ToString()
        {
            return string.Format("({0})", score);
        }

        private readonly double score;
    }
}
