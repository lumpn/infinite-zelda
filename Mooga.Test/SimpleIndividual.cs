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
            switch (attribute)
            {
                case 0: return distance;
                case 1: return score;
            }

            Debug.Fail();
            return 0.0;
        }

        public void SetScore(int attribute, double value)
        {
            Debug.Assert(attribute == 0);
            distance = value;
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", distance, score);
        }

        private double distance;
        private readonly double score;
    }
}
