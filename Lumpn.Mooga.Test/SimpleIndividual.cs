namespace Lumpn.Mooga.Test
{
    public sealed class SimpleIndividual : Individual
    {
        private readonly double score;
        private readonly Genome genome;

        public SimpleIndividual(double score, Genome genome = null)
        {
            this.score = score;
            this.genome = genome;
        }

        public Genome Genome { get { return genome; } }

        public double GetScore(int attribute)
        {
            return score;
        }

        public override string ToString()
        {
            return string.Format("({0})", score);
        }
    }
}
