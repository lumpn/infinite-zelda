using Lumpn.Mooga;
using Lumpn.Utils;
using System.Collections.Generic;

namespace Lumpn.ZeldaMooga
{
    public sealed class SwitchGene : ZeldaGene
    {
        public SwitchGene(ZeldaConfiguration configuration, RandomNumberGenerator random)
        {
            super(configuration);
            this.switchLocation = RandomLocation(random);
        }

        public SwitchGene mutate(RandomNumberGenerator random)
        {
            return new SwitchGene(getConfiguration(), random);
        }

        public int countErrors(List<ZeldaGene> genes)
        {

            // find a piston
            for (ZeldaGene gene : genes)
            {
                if (gene is PistonGene) return 0;
            }

            // no piston -> useless switch
            return 1;
        }

        public void express(ZeldaDungeonBuilder builder)
        {
            VariableLookup lookup = builder.lookup();
            builder.addScript(switchLocation, ZeldaScripts.createSwitch(lookup));
        }

        public String toString()
        {
            return String.format("switch %d", switchLocation);
        }

        private readonly int switchLocation;
    }
}

