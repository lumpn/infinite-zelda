using Lumpn.Dungeon;
using Lumpn.Dungeon.Scripts;

namespace Lumpn.ZeldaMooga
{
    public sealed class OneWayGene : ZeldaGene
    {
        private readonly int wayStart, wayEnd;

        public OneWayGene(ZeldaConfiguration configuration)
            : base(configuration)
        {
            this.wayStart = configuration.RandomLocation();
            this.wayEnd = configuration.RandomLocation(wayStart);
        }

        public override Gene Mutate()
        {
            return new OneWayGene(configuration);
        }

        public override void Express(CrawlerBuilder builder, VariableLookup lookup)
        {
            builder.AddDirectedTransition(wayStart, wayEnd, NoOpScript.instance);
        }

        public override string ToString()
        {
            return string.Format("{0}->{1}", wayStart, wayEnd);
        }
    }
}
