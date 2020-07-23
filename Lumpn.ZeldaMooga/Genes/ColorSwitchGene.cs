using Lumpn.ZeldaDungeon;

namespace Lumpn.ZeldaMooga
{
    public sealed class ColorSwitchGene : ZeldaGene
    {
        private readonly int switchType;
        private readonly int switchLocation;

        public ColorSwitchGene(ZeldaConfiguration configuration)
            : base(configuration)
        {
            this.switchType = configuration.RandomSwitchType();
            this.switchLocation = RandomLocation();
        }

        public override Gene Mutate()
        {
            return new ColorSwitchGene(Configuration);
        }

        public override void Express(ZeldaDungeonBuilder builder)
        {
            builder.AddScript(switchLocation, ZeldaScripts.CreateColorSwitch(switchType, builder.Lookup));
        }

        public override string ToString()
        {
            return string.Format("switch {0} at {1}", switchType, switchLocation);
        }
    }
}
