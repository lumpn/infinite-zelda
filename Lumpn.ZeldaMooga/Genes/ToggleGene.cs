using Lumpn.Dungeon;
using Lumpn.Dungeon.Scripts;

namespace Lumpn.ZeldaMooga
{
    public sealed class ToggleGene : ZeldaGene
    {
        private readonly string switchName;
        private readonly int switchLocation;

        public ToggleGene(ZeldaConfiguration configuration, string switchName)
            : base(configuration)
        {
            this.switchName = switchName;
            this.switchLocation = configuration.RandomLocation();
        }

        public override Gene Mutate()
        {
            return new ToggleGene(configuration, switchName);
        }

        public override void Express(CrawlerBuilder builder, VariableLookup lookup)
        {
            var script = new ToggleScript(switchName, lookup);
            builder.AddScript(switchLocation, script);
        }

        public override string ToString()
        {
            return string.Format("{0} at {1}", switchName, switchLocation);
        }
    }
}
