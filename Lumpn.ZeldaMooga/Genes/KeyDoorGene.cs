using Lumpn.Mooga;
using Lumpn.Utils;
using Lumpn.ZeldaDungeon;
using System.Collections.Generic;
using System;

namespace Lumpn.ZeldaMooga
{
    public sealed class KeyDoorGene : ZeldaGene
    {
        private readonly int keyType;
        private readonly int keyLocation;
        private readonly int doorStart, doorEnd;

        public KeyDoorGene(ZeldaConfiguration configuration, RandomNumberGenerator random)
            : base(configuration)
        {
            this.keyType = configuration.RandomKeyType(random);
            this.keyLocation = RandomLocation(random);
            this.doorStart = RandomLocation(random);
            this.doorEnd = DifferentLocation(random, doorStart);
        }

        private KeyDoorGene(ZeldaConfiguration configuration, int keyType, int keyLocation, int doorStart, int doorEnd)
            : base(configuration)
        {
            this.keyType = keyType;
            this.keyLocation = keyLocation;
            this.doorStart = doorStart;
            this.doorEnd = doorEnd;
        }

        public override Gene Mutate(RandomNumberGenerator random)
        {
            return new KeyDoorGene(Configuration, random);
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
