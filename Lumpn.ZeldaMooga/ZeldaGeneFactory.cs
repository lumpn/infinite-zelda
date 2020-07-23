using Lumpn.Utils;

namespace Lumpn.ZeldaMooga
{
    public sealed class ZeldaGeneFactory : GeneFactory<ZeldaGene>
    {
        public ZeldaGene CreateGene(ZeldaConfiguration configuration)
        {
            switch (configuration.RandomGeneType())
            {
                case 0: return new ColorPistonGene(configuration );
                case 1: return new ColorSwitchGene(configuration);
                case 2: return new KeyDoorGene(configuration);
                case 3: return new ObstacleGene(configuration);
                case 4: return new OneWayGene(configuration);
                case 5: return new ToolGene(configuration);
            }

            Debug.Fail();
            return null;
        }
    }
}
