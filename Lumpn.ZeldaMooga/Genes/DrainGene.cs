using Lumpn.Dungeon;
using Lumpn.Dungeon.Scripts;

namespace Lumpn.ZeldaMooga
{
    public sealed class DrainGene : ZeldaGene
    {
        private readonly int drainLocation, a, b, c, d;

        public DrainGene(ZeldaConfiguration configuration)
            : base(configuration)
        {
            drainLocation = configuration.RandomLocation();
            a = configuration.RandomLocation();
            b = configuration.RandomLocation();
            c = configuration.RandomLocation();
            d = configuration.RandomLocation();
        }

        public override Gene Mutate()
        {
            return new DrainGene(configuration);
        }

        public override void Express(CrawlerBuilder builder, VariableLookup lookup)
        {
            var drainId = lookup.Unique("drain state");

            var drainScript = new SetScript(1, drainId);
            builder.AddScript(drainLocation, drainScript);

            builder.AddUndirectedTransition(a, b, new EqualsScript(0, "flooded", drainId));
            builder.AddUndirectedTransition(c, d, new EqualsScript(1, "drained", drainId));
        }

        public override string ToString()
        {
            return string.Format("drain at {0}, {1}--{2}, {3}--{4}", drainLocation, a, b, c, d);
        }
    }
}
