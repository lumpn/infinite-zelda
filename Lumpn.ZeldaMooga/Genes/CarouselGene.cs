using Lumpn.Dungeon;
using Lumpn.Dungeon.Scripts;

namespace Lumpn.ZeldaMooga
{
    public sealed class CarouselGene : ZeldaGene
    {
        private readonly int a, b, c, d;

        public CarouselGene(ZeldaConfiguration configuration)
            : base(configuration)
        {
            a = configuration.RandomLocation();
            b = configuration.RandomLocation();
            c = configuration.RandomLocation();
            d = configuration.RandomLocation();
        }

        public override Gene Mutate()
        {
            return new CarouselGene(configuration);
        }

        public override void Express(CrawlerBuilder builder, VariableLookup lookup)
        {
            //   c
            // b + d
            //   a

            var carouselId = lookup.Unique("carousel state");
            var toggleScript = new ToggleScript(carouselId);
            var turnLeftScript = new EqualsScript(0, "turn left", carouselId);
            var turnRightScript = new EqualsScript(1, "turn right", carouselId);
            var carousel1 = new AndScript(turnLeftScript, toggleScript);
            var carousel2 = new AndScript(turnRightScript, toggleScript);

            builder.AddDirectedTransition(a, b, carousel1);
            builder.AddDirectedTransition(b, c, carousel1);
            builder.AddDirectedTransition(c, d, carousel1);
            builder.AddDirectedTransition(d, a, carousel1);

            builder.AddDirectedTransition(a, d, carousel2);
            builder.AddDirectedTransition(d, c, carousel2);
            builder.AddDirectedTransition(c, b, carousel2);
            builder.AddDirectedTransition(b, a, carousel2);
        }
    }
}
