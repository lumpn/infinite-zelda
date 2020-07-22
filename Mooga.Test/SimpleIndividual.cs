namespace Lumpn.Mooga.Test
{
    public sealed class SimpleIndividual : Individual
    {
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

        private readonly double score;
        private readonly Genome genome;
    }
}
