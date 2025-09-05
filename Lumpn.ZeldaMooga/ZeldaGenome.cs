using Lumpn.Dungeon;
using Lumpn.Mooga;
using Lumpn.Utils;
using System.Collections.Generic;

namespace Lumpn.ZeldaMooga
{
    public sealed class ZeldaGenome : Genome
    {
        private readonly ZeldaConfiguration configuration;
        private readonly ZeldaGeneFactory factory;
        private readonly List<ZeldaGene> genes;

        public ZeldaGenome(ZeldaConfiguration configuration, ZeldaGeneFactory factory)
        {
            this.configuration = configuration;
            this.factory = factory;

            int count = configuration.CalcNumInitialGenes();
            this.genes = GeneUtils.Generate(count, factory, configuration);
        }

        private ZeldaGenome(ZeldaConfiguration configuration, ZeldaGeneFactory factory, List<ZeldaGene> genes)
        {
            this.configuration = configuration;
            this.factory = factory;
            this.genes = genes;
        }

        public Pair<Genome> Crossover(Genome o, RandomNumberGenerator random)
        {
            ZeldaGenome other = (ZeldaGenome)o;

            // randomly distribute genes
            var newGenesA = new List<ZeldaGene>(genes);
            var newGenesB = new List<ZeldaGene>(other.genes);
            GeneUtils.Crossover(newGenesA, newGenesB, random);

            // assemble offsprings
            var a = new ZeldaGenome(configuration, factory, newGenesA);
            var b = new ZeldaGenome(configuration, factory, newGenesB);
            return new Pair<Genome>(a, b);
        }

        public Genome Mutate(RandomNumberGenerator random)
        {
            // mutate genes
            var newGenes = new List<ZeldaGene>(genes);
            GeneUtils.Mutate(newGenes, factory, configuration);

            // assemble offspring
            return new ZeldaGenome(configuration, factory, newGenes);
        }

        public void Express(CrawlerBuilder builder, VariableLookup lookup)
        {
            foreach (var gene in genes)
            {
                gene.Express(builder, lookup);
            }
        }

        public override string ToString()
        {
            return string.Join(", ", genes);
        }
    }
}
