using Lumpn.Dungeon;

namespace Lumpn.ZeldaMooga
{
    public abstract class ZeldaGene : Gene
    {
        protected readonly ZeldaConfiguration configuration;

        public ZeldaGene(ZeldaConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public abstract Gene Mutate();

        public abstract void Express(CrawlerBuilder builder, VariableLookup lookup);
    }
}
