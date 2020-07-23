using Lumpn.Mooga;
using Lumpn.Utils;
using System.Collections.Generic;
using Lumpn.ZeldaDungeon;

namespace Lumpn.ZeldaMooga
{
    public sealed class ItemGene : ZeldaGene
    {
        private readonly int item;
        private readonly int itemLocation;

        public ItemGene(ZeldaConfiguration configuration, RandomNumberGenerator random)
            : base(configuration)
        {
            this.item = configuration.RandomItem(random);
            this.itemLocation = RandomLocation(random);
        }

        private ItemGene(ZeldaConfiguration configuration, int item, int itemLocation)
            : base(configuration)
        {
            this.item = item;
            this.itemLocation = itemLocation;
        }

        public override Gene Mutate(RandomNumberGenerator random)
        {
            return new ItemGene(Configuration, random);
        }

        public override int CountErrors(List<ZeldaGene> genes)
        {
            // find duplicates
            int numErrors = 0;
            foreach (ZeldaGene gene in genes)
            {
                if (gene is ItemGene)
                {
                    ItemGene other = (ItemGene)gene;
                    if (other != this && other.equals(this)) numErrors++;
                }
            }

            // find obstacle
            foreach (ZeldaGene gene in genes)
            {
                if (gene is ObstacleGene)
                {

                    ObstacleGene other = (ObstacleGene)gene;
                    if (other.requiredItem() == item) return numErrors;
                }
            }

            // no obstacle -> useless item
            return numErrors + 1;
        }

        public override void Express(ZeldaDungeonBuilder builder)
        {
            VariableLookup lookup = builder.lookup();

            // spawn item
            builder.addScript(itemLocation, ZeldaScripts.createItem(item, lookup));
        }


        public override string ToString()
        {
            return string.Format("item {0} at {1}", item, itemLocation);
        }
    }
}
