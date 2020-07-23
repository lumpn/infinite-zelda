using Lumpn.Mooga;
using Lumpn.Utils;
using System.Collections.Generic;
using System;

namespace Lumpn.ZeldaMooga
{
    public sealed class KeyDoorGene : ZeldaGene
    {
        public KeyDoorGene(ZeldaConfiguration configuration, RandomNumberGenerator random)
            : base(configuration)
        {
            this.keyLocation = RandomLocation(random);
            int a = RandomLocation(random);
            int b = DifferentLocation(a, random);
            this.doorStart = Math.Min(a, b);
            this.doorEnd = Math.Max(a, b);
        }

        private KeyDoorGene(ZeldaConfiguration configuration, int keyLocation, int doorStart, int doorEnd)
            : base(configuration)
        {
            this.keyLocation = keyLocation;
            this.doorStart = doorStart;
            this.doorEnd = doorEnd;
        }

        public KeyDoorGene Mutate(RandomNumberGenerator random)
        {
            return new KeyDoorGene(Configuration, random);
        }

        public int CountErrors(List<ZeldaGene> genes)
        {
            return 0;
        }

        public void Wxpress(ZeldaDungeonBuilder builder)
        {
            VariableLookup lookup = builder.lookup();

            // spawn key
            builder.addScript(keyLocation, ZeldaScripts.createKey(lookup));

            // spawn door
            builder.addUndirectedTransition(doorStart, doorEnd, ZeldaScripts.createDoor(lookup));
        }


        public String toString()
        {
            return String.format("key %d, door %d--%d", keyLocation, doorStart, doorEnd);
        }

        // location of key
        private readonly int keyLocation;

        // transition location of door
        private readonly int doorStart, doorEnd;
    }
}
