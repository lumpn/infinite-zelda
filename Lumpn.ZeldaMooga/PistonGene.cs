using Lumpn.Mooga;
using Lumpn.Utils;
using System.Collections.Generic;

namespace Lumpn.ZeldaMooga
{
    public sealed class PistonGene : ZeldaGene
    {
        private static enum Color
        {
            COLOR_RED, COLOR_BLUE,
        }

        private static String ColorToString(Color color)
        {
            switch (color)
            {
                case COLOR_RED:
                    return "red";
                case COLOR_BLUE:
                    return "blue";
                default:
                    return "?";
            }
        }

        private static Color randomColor(RandomNumberGenerator random)
        {
            if (random.nextBoolean())
            {
                return Color.COLOR_RED;
            }
            return Color.COLOR_BLUE;
        }

        public PistonGene(ZeldaConfiguration configuration, RandomNumberGenerator random)
        {
            super(configuration);
            this.color = randomColor(random);
            int a = randomLocation(random);
            int b = differentLocation(a, random);
            this.pistonStart = Math.min(a, b);
            this.pistonEnd = Math.max(a, b);
        }

        public PistonGene mutate(RandomNumberGenerator random)
        {
            return new PistonGene(getConfiguration(), random);
        }

        public int countErrors(List<ZeldaGene> genes)
        {

            // find a switch
            for (ZeldaGene gene : genes)
            {
                if (gene is SwitchGene) return 0;
            }

            // no switch -> no good
            return 1;
        }

        public void express(ZeldaDungeonBuilder builder)
        {
            VariableLookup lookup = builder.lookup();
            ZeldaScript script;
            switch (color)
            {
                case COLOR_RED:
                    script = ZeldaScripts.createRedPiston(lookup);
                    break;
                case COLOR_BLUE:
                    script = ZeldaScripts.createBluePiston(lookup);
                    break;
                default:
                    script = IdentityScript.INSTANCE;
                    assert false;
            }
            builder.addUndirectedTransition(pistonStart, pistonEnd, script);
        }

        public String toString()
        {
            return String.format("%s piston %d--%d", ColorToString(color), pistonStart, pistonEnd);
        }

        /**
         * Piston color
         */
        private readonly Color color;

        /**
         * Piston transition location
         */
        private readonly int pistonStart, pistonEnd;
    }
}
