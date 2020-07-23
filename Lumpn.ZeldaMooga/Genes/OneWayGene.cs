using Lumpn.ZeldaDungeon;

namespace Lumpn.ZeldaMooga
{
    public sealed class OneWayGene : ZeldaGene
    {
        private readonly int wayStart, wayEnd;

        public OneWayGene(ZeldaConfiguration configuration)
            : base(configuration)
        {
            this.wayStart = RandomLocation();
            this.wayEnd = RandomLocation(wayStart);
        }

        public override Gene Mutate()
        {
            return new OneWayGene(Configuration);
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
