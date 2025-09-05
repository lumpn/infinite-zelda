using Lumpn.Dungeon;
using Lumpn.Dungeon.Scripts;

namespace Lumpn.ZeldaMooga
{
    public sealed class TrainTrackGene : ZeldaGene
    {
        private readonly int switchLocation;
        private readonly int junctionLocation, locationA, locationB;

        public TrainTrackGene(ZeldaConfiguration configuration)
            : base(configuration)
        {
            switchLocation = configuration.RandomLocation();
            junctionLocation = configuration.RandomLocation();
            locationA = configuration.RandomLocation(junctionLocation);
            locationB = configuration.RandomLocation(junctionLocation);
        }

        public override Gene Mutate()
        {
            return new TrainTrackGene(configuration);
        }

        public override void Express(CrawlerBuilder builder, VariableLookup lookup)
        {
            var switchId = lookup.Unique("track state");
            builder.AddScript(switchLocation, new ToggleScript(switchId));

            var scriptA = new EqualsScript(0, "track A", switchId);
            var scriptB = new EqualsScript(1, "track B", switchId);
            builder.AddUndirectedTransition(junctionLocation, locationA, scriptA);
            builder.AddUndirectedTransition(junctionLocation, locationB, scriptB);
        }

        public override string ToString()
        {
            return string.Format("track switch at {0}, {1}--{2}, {1}--{3}", switchLocation, junctionLocation, locationA, locationB);
        }
    }
}
