using Lumpn.Utils;
using Lumpn.ZeldaDungeon;

namespace Lumpn.ZeldaMooga
{
    public sealed class OneWayGene : ZeldaGene
    {
        private readonly int wayStart, wayEnd;

        public OneWayGene(ZeldaConfiguration configuration, RandomNumberGenerator random)
            : base(configuration)
        {
            this.wayStart = RandomLocation(random);
            this.wayEnd = DifferentLocation(random, wayStart);
        }

        public OneWayGene(ZeldaConfiguration configuration, int wayStart, int wayEnd)
            : base(configuration)
        {
            this.wayStart = wayStart;
            this.wayEnd = wayEnd;
        }

        public override Gene Mutate(RandomNumberGenerator random)
        {
            return new OneWayGene(Configuration, random);
        }

        public override void Express(ZeldaDungeonBuilder builder)
        {
            builder.AddDirectedTransition(wayStart, wayEnd, IdentityScript.Default);
        }

        public override string ToString()
        {
            return string.Format("{0}->{1}", wayStart, wayEnd);
        }
    }
}
