using Lumpn.Utils;

namespace Lumpn.Mooga.Test
{
    public sealed class ParetoIndividual : Individual
    {
        public ParetoIndividual(double score1, double score2, double score3)
        {
            this.score1 = score1;
            this.score2 = score2;
            this.score3 = score3;
        }

        public Genome Genome { get { return null; } } // not needed for test

        public double GetScore(int attribute)
        {
            switch (attribute)
            {
                case 0: return distance;
                case 1: return score1;
                case 2: return score2;
                case 3: return score3;
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
            return string.Format("({0}, {1}, {2}, {3})", distance, score1, score2, score3);
        }

        private double distance;
        private readonly double score1, score2, score3;
    }
}
