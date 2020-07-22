using System.Collections.Generic;
using Lumpn.Mooga;

/// Immutable Zelda genome
public sealed class ZeldaGenome : IGenome
{
    public ZeldaGenome(ZeldaConfiguration configuration, IRandom random)
    {
        this.configuration = configuration;

        // add some more genes
        int count = configuration.calcNumInitialGenes(random);
        this.genes = CollectionUtils.immutable(GeneUtils.generate(count, factory, configuration, random));
    }

    private ZeldaGenome(ZeldaConfiguration configuration, List<ZeldaGene> genes)
    {
        this.configuration = configuration;
        this.genes = CollectionUtils.immutable(genes);
    }

    public IEnumerable<Genome> Crossover(IGenome o, IRandom random)
    {
        ZeldaGenome other = (ZeldaGenome)o;

        // randomly distribute genes
        Pair<List<ZeldaGene>> distributedGenes = CollectionUtils.distribute(genes, other.genes, random);

        // assemble offsprings
        ZeldaGenome x = new ZeldaGenome(configuration, distributedGenes.first());
        ZeldaGenome y = new ZeldaGenome(configuration, distributedGenes.second());
        return new Pair<Genome>(x, y);
    }

    @Override
    public Genome mutate(Random random)
    {

        // mutate genes
        List<ZeldaGene> newGenes = GeneUtils.mutate(genes, factory, configuration, random);

        // assemble offspring
        return new ZeldaGenome(configuration, newGenes);
    }

    public int size()
    {
        return genes.size();
    }

    public int countErrors()
    {
        int numErrors = 0;
        for (ZeldaGene gene : genes)
        {
            numErrors += gene.countErrors(genes);
        }
        return numErrors;
    }

    public void express(ZeldaPuzzleBuilder builder)
    {
        for (ZeldaGene gene : genes)
        {
            gene.express(builder);
        }
    }

    @Override
    public int hashCode()
    {
        return genes.hashCode();
    }

    @Override
    public boolean equals(Object obj)
    {
        if (this == obj) return true;
        if (obj == null) return false;
        if (!(obj instanceof ZeldaGenome)) return false;
        ZeldaGenome other = (ZeldaGenome)obj;
        if (!genes.equals(other.genes)) return false;
        return true;
    }

    @Override
    public String toString()
    {
        return String.format("%s", genes);
    }

    private readonly ZeldaConfiguration configuration;

	private readonly List<ZeldaGene> genes;

    private static readonly ZeldaGeneFactory factory = new ZeldaGeneFactory();
}
