namespace Lumpn.ZeldaMooga
{
    public sealed class ZeldaGeneFactory : GeneFactory<ZeldaGene>
    {
        private readonly ZeldaGene[] prototypes;

        public ZeldaGeneFactory(ZeldaConfiguration configuration)
        {
            prototypes = CreateGenes(configuration);
        }

        public ZeldaGene CreateGene(ZeldaConfiguration configuration)
        {
            var idx = configuration.Random(prototypes.Length);
            var prototype = prototypes[idx];
            return (ZeldaGene)prototype.Mutate();
        }

        private static ZeldaGene[] CreateGenes(ZeldaConfiguration configuration)
        {
            var genes = new ZeldaGene[]
            {
                new OneWayGene(configuration),
                new KeyDoorGene(configuration, "small key", "door"),
                new ToolGene(configuration, "tool"),
                new ObstacleGene(configuration, "tool", "obstacle"),
                new ToolGene(configuration, "boss key"),
                new ObstacleGene(configuration, "boss key", "boss door"),
                new ToggleGene(configuration, "color switch"),
                new EqualsGene(configuration, "color switch", "blue piston", 0),
                new EqualsGene(configuration, "color switch", "red piston", 1),
                new WaterLevelGene(configuration, "water"),
                new EqualsGene(configuration, "water", "water==0", 0),
                new EqualsGene(configuration, "water", "water==1", 1),
                new EqualsGene(configuration, "water", "water==2", 2),
                new NotEqualsGene(configuration, "water", "water!=0", 0),
                new NotEqualsGene(configuration, "water", "water!=1", 1),
                new NotEqualsGene(configuration, "water", "water!=2", 2),
                new TrainTrackGene(configuration),
                new CarouselGene(configuration),
                new DrainGene(configuration),
            };
            return genes;
        }
    }
}
