using Lumpn.Utils;

namespace Lumpn.Mooga.Test
{
    public sealed class ParetoGenome : Genome
    {
        public readonly double value1, value2, value3;

        public ParetoGenome(double value1, double value2, double value3)
        {
            this.value1 = value1;
            this.value2 = value2;
            this.value3 = value3;
        }

        public Pair<Genome> Crossover(Genome other, RandomNumberGenerator random)
        {
            var o = (ParetoGenome)other;

            var split = random.NextInt(3);
            var a1 = (split < 1) ? value1 : o.value1;
            var a2 = (split < 2) ? value2 : o.value2;
            var a3 = (split < 3) ? value3 : o.value3;
            var b1 = (split < 1) ? o.value1 : value1;
            var b2 = (split < 2) ? o.value2 : value2;
            var b3 = (split < 3) ? o.value3 : value3;

            var a = new ParetoGenome(a1, a2, a3);
            var b = new ParetoGenome(b1, b2, b3);
            return new Pair<Genome>(a, b);
        }

        public Genome Mutate(RandomNumberGenerator random)
        {
            return new ParetoGenome(
                    value1 + random.Range(-1.0, 1.0),
                    value2 + random.Range(-1.0, 1.0),
                    value3 + random.Range(-1.0, 1.0)
                );
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", value1, value2, value3);
        }
    }
}
