using Lumpn.Utils;
using Lumpn.ZeldaDungeon;

namespace Lumpn.ZeldaMooga
{
    public abstract class ZeldaGene : Gene
    {
        private readonly ZeldaConfiguration configuration;

        public ZeldaConfiguration Configuration { get { return configuration; } }

        public ZeldaGene(ZeldaConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public int RandomLocation(RandomNumberGenerator random)
        {
            return configuration.RandomLocation(random);
        }

        public int DifferentLocation(RandomNumberGenerator random, int otherLocation)
        {
            int location;
            do
            {
                location = configuration.RandomLocation(random);
            }
            while (location == otherLocation);

            return location;
        }

        public abstract Gene Mutate(RandomNumberGenerator random);

        public abstract void Express(ZeldaDungeonBuilder builder);
    }
}
