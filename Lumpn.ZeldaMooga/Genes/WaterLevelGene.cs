using Lumpn.Dungeon;
using Lumpn.Dungeon.Scripts;

namespace Lumpn.ZeldaMooga
{
    public sealed class WaterLevelGene : ZeldaGene
    {
        private readonly string waterLevelName;
        private readonly int location1, location2, location3;

        public WaterLevelGene(ZeldaConfiguration configuration, string waterLevelName)
            : base(configuration)
        {
            this.waterLevelName = waterLevelName;
            this.location1 = configuration.RandomLocation();
            this.location2 = configuration.RandomLocation();
            this.location3 = configuration.RandomLocation();
        }

        public override Gene Mutate()
        {
            return new WaterLevelGene(configuration, waterLevelName);
        }

        public override void Express(CrawlerBuilder builder, VariableLookup lookup)
        {
            var script1 = new SetScript(0, waterLevelName, lookup);
            var script2 = new SetScript(1, waterLevelName, lookup);
            var script3 = new SetScript(2, waterLevelName, lookup);

            builder.AddScript(location1, script1);
            builder.AddScript(location2, script2);
            builder.AddScript(location3, script3);
        }

        public override string ToString()
        {
            return string.Format("{0}:=0 at {1}, {0}:=1 at {2}, {0}:=2 at {3}", waterLevelName, location1, location2, location3);
        }
    }
}
