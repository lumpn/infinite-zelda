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

        public int RandomLocation()
        {
            return configuration.RandomLocation();
        }

        /// random location different from the other location
        public int RandomLocation(int otherLocation)
        {
            int location;
            do
            {
                location = configuration.RandomLocation();
            }
            while (location == otherLocation);

            return location;
        }

        public abstract Gene Mutate();

        public abstract void Express(ZeldaDungeonBuilder builder);
    }
}
