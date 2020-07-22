using Lumpn.Utils;

namespace Lumpn.Mooga.Test
{
    public sealed class SimpleGenome : Genome
    {
        public SimpleGenome(double value)
        {
            this.value = value;
        }

        public Pair<Genome> Crossover(Genome other, RandomNumberGenerator random)
        {
            var o = (SimpleGenome)other;

            var child1 = new SimpleGenome(MathUtils.Lerp(value, o.value, random.NextDouble()));
            var child2 = new SimpleGenome(MathUtils.Lerp(value, o.value, random.NextDouble()));
            return new Pair<Genome>(child1, child2);
        }

        public Genome Mutate(RandomNumberGenerator random)
        {
            return new SimpleGenome(value + random.Range(-1.0, 1.0));
        }

        public override string ToString()
        {
            return string.Format("({0})", value);
        }

        public readonly double value;
    }
}
