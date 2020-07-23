using Lumpn.Mooga;
using Lumpn.Utils;
using System.Collections.Generic;

namespace Lumpn.ZeldaMooga
{
    public sealed class ObstacleGene : ZeldaGene
    {
        /// tool to overcome obstacle
        private readonly int requiredItem;

        /// obstacle transition location
        private readonly int obstacleStart, obstacleEnd;

        public ObstacleGene(ZeldaConfiguration configuration, RandomNumberGenerator random)
        {
            super(configuration);
            this.requiredItem = configuration.randomItem(random);
            int a = randomLocation(random);
            int b = differentLocation(a, random);
            this.obstacleStart = Math.min(a, b);
            this.obstacleEnd = Math.max(a, b);
        }

        public int requiredItem()
        {
            return requiredItem;
        }

        public ObstacleGene mutate(RandomNumberGenerator random)
        {
            return new ObstacleGene(getConfiguration(), random);
        }

        public int countErrors(List<ZeldaGene> genes)
        {

            // find item
            for (ZeldaGene gene : genes)
            {
                if (gene is ItemGene)
                {
                    ItemGene other = (ItemGene)gene;
                    if (other.item() == requiredItem) return 0;
                }
            }

            return 1;
        }

        public void express(ZeldaDungeonBuilder builder)
        {
            VariableLookup lookup = builder.lookup();
            builder.addUndirectedTransition(obstacleStart, obstacleEnd, ZeldaScripts.createObstacle(requiredItem, lookup));
        }

        public String toString()
        {
            return String.format("obstacle %s %d--%d", requiredItem, obstacleStart, obstacleEnd);
        }
    }
}
