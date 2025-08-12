using Lumpn.Utils;

namespace Lumpn.Mooga.Test
{
    public sealed class ParetoGenome : Genome
    {
        private readonly double score1, score2, score3;

        public ParetoGenome(double score1, double score2, double score3)
        {
            this.score1 = score1;
            this.score2 = score2;
            this.score3 = score3;
        }

        public Pair<Genome> Crossover(Genome other, RandomNumberGenerator random)
        {
            var o = (ParetoGenome)other;
            var a = new ParetoGenome(
                    MathUtils.Lerp(score1, o.score1, random.NextDouble()),
                    MathUtils.Lerp(score2, o.score2, random.NextDouble()),
                    MathUtils.Lerp(score3, o.score3, random.NextDouble())
                );
            var b = new ParetoGenome(
                    MathUtils.Lerp(score1, o.score1, random.NextDouble()),
                    MathUtils.Lerp(score2, o.score2, random.NextDouble()),
                    MathUtils.Lerp(score3, o.score3, random.NextDouble())
                );
            return new Pair<Genome>(a, b);
        }

        public Genome Mutate(RandomNumberGenerator random)
        {
            return new ParetoGenome(
                    score1 + random.Range(-1.0, 1.0),
                    score2 + random.Range(-1.0, 1.0),
                    score3 + random.Range(-1.0, 1.0)
                );
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", score1, score2, score3);
        }
    }
}
