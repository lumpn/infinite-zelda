using Lumpn.Dungeon;
using Lumpn.ZeldaDungeon;
using System;

namespace Lumpn.ZeldaMooga
{
    public sealed class ColorPistonGene : ZeldaGene
    {
        private readonly int switchType;
        private readonly int switchColor;
        private readonly int pistonStart, pistonEnd;

        public ColorPistonGene(ZeldaConfiguration configuration)
            : base(configuration)
        {
            this.switchType = configuration.RandomSwitchType();
            this.switchColor = configuration.RandomSwitchColor();

            int a = RandomLocation();
            int b = RandomLocation(a);
            this.pistonStart = Math.Min(a, b);
            this.pistonEnd = Math.Max(a, b);
        }

        public override Gene Mutate()
        {
            return new ColorPistonGene(Configuration);
        }

        public override void Express(ZeldaDungeonBuilder builder)
        {
            builder.AddUndirectedTransition(pistonStart, pistonEnd, CreateScript(switchType, switchColor, builder.Lookup));
        }

        public override string ToString()
        {
            return string.Format("piston type {0} color {1} {2}--{3}", switchType, switchColor, pistonStart, pistonEnd);
        }

        private static Script CreateScript(int switchType, int switchColor, VariableLookup lookup)
        {
            return ZeldaScripts.CreateColorPiston(switchType, switchColor, lookup);
        }
    }
}