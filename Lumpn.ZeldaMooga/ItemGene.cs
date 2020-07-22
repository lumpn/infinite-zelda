using Lumpn.Mooga;

public sealed class ItemGene : ZeldaGene
{
    public ItemGene(ZeldaConfiguration configuration, IRandom random)
        : base(configuration)
    {
        this.item = configuration.randomItem(random);
        this.itemLocation = randomLocation(random);
    }

    private ItemGene(ZeldaConfiguration configuration, int item, int itemLocation)
        : base(configuration)
    {
        this.item = item;
        this.itemLocation = itemLocation;
    }

    public int item()
    {
        return item;
    }

    public ItemGene mutate(Random random)
    {
        return new ItemGene(getConfiguration(), random);
    }

    public int countErrors(List<ZeldaGene> genes)
    {

        // find duplicates
        int numErrors = 0;
        for (ZeldaGene gene : genes)
        {
            if (gene instanceof ItemGene) {
            ItemGene other = (ItemGene)gene;
            if (other != this && other.equals(this)) numErrors++;
        }
    }

		// find obstacle
		for (ZeldaGene gene : genes) {
			if (gene instanceof ObstacleGene) {

                ObstacleGene other = (ObstacleGene)gene;
				if (other.requiredItem() == item) return numErrors;
			}
		}

		// no obstacle -> useless item
		return numErrors + 1;
	}

    public void express(ZeldaPuzzleBuilder builder)
{
    VariableLookup lookup = builder.lookup();

    // spawn item
    builder.addScript(itemLocation, ZeldaScripts.createItem(item, lookup));
}

public int hashCode()
{
    final int prime = 31;
    int result = 1;
    result = prime * result + item;
    result = prime * result + itemLocation;
    return result;
}

public boolean equals(Object obj)
{
    if (this == obj) return true;
    if (obj == null) return false;
    if (!(obj instanceof ItemGene)) return false;
    ItemGene other = (ItemGene)obj;
    if (item != other.item) return false;
    if (itemLocation != other.itemLocation) return false;
    return true;
}

public String toString()
{
    return String.format("item %d %d", item, itemLocation);
}

// item
private readonly int item;

// location of item
private readonly int itemLocation;
}
