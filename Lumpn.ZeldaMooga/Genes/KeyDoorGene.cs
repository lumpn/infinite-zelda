using Lumpn.Dungeon;
using Lumpn.Dungeon.Scripts;
using System;

namespace Lumpn.ZeldaMooga
{
    public sealed class KeyDoorGene : ZeldaGene
    {
        private readonly string keyName, doorName;
        private readonly int keyLocation;
        private readonly int doorStart, doorEnd;

        public KeyDoorGene(ZeldaConfiguration configuration, string keyName, string doorName)
            : base(configuration)
        {
            this.keyLocation = configuration.RandomLocation();
            int a = configuration.RandomLocation();
            int b = configuration.RandomLocation(a);
            this.doorStart = Math.Min(a, b);
            this.doorEnd = Math.Max(a, b);
        }

        public override Gene Mutate()
        {
            return new KeyDoorGene(configuration, keyName, doorName);
        }

        public override void Express(CrawlerBuilder builder, VariableLookup lookup)
        {
            // spawn key
            var keyScript = new AcquireScript(keyName, lookup);
            builder.AddScript(keyLocation, keyScript);

            // spawn door
            var doorScript = new ConsumeScript(doorName, keyName, lookup);
            builder.AddUndirectedTransition(doorStart, doorEnd, doorScript);
        }

        public override string ToString()
        {
            return string.Format("{0} at {1}, {2} {3}--{4}", keyName, keyLocation, doorName, doorStart, doorEnd);
        }
    }
}
