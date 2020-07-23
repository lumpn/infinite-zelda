using System;
using Lumpn.ZeldaDungeon;

namespace Lumpn.ZeldaMooga
{
    public sealed class KeyDoorGene : ZeldaGene
    {
        private readonly int keyType;
        private readonly int keyLocation;
        private readonly int doorStart, doorEnd;

        public KeyDoorGene(ZeldaConfiguration configuration)
            : base(configuration)
        {
            this.keyType = configuration.RandomKeyType();
            this.keyLocation = RandomLocation();
            int a = RandomLocation();
            int b = RandomLocation(a);
            this.doorStart = Math.Min(a, b);
            this.doorEnd = Math.Max(a, b);
        }

        public override Gene Mutate()
        {
            return new KeyDoorGene(Configuration);
        }

        public override void Express(ZeldaDungeonBuilder builder)
        {
            // spawn key
            builder.AddScript(keyLocation, ZeldaScripts.CreateKey(keyType, builder.Lookup));

            // spawn door
            builder.AddUndirectedTransition(doorStart, doorEnd, ZeldaScripts.CreateDoor(keyType, builder.Lookup));
        }

        public override string ToString()
        {
            return string.Format("key {0} at {1}, door {2}--{3}", keyType, keyLocation, doorStart, doorEnd);
        }
    }
}
