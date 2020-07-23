using Lumpn.Mooga;
using Lumpn.Mooga;
using Lumpn.Utils;
using System.Collections.Generic;

namespace Lumpn.ZeldaMooga
{
    public sealed class ZeldaGeneFactory : GeneFactory<ZeldaGene>
    {
        public ZeldaGene CreateGene(ZeldaConfiguration configuration, RandomNumberGenerator random)
        {
            // TODO: support weighted probability
            switch (random.NextInt(7))
            {
                case 0: return new TwoWayGene(configuration, random);
                case 1: return new OneWayGene(configuration, random);
                case 2: return new KeyDoorGene(configuration, random);
                case 3: return new SwitchGene(configuration, random);
                case 4: return new PistonGene(configuration, random);
                case 5: return new ItemGene(configuration, random);
                case 6: return new ObstacleGene(configuration, random);
                default: return new TwoWayGene(configuration, random);
            }
        }
    }
}
