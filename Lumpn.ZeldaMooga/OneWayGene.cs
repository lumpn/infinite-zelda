using Lumpn.Mooga;
using Lumpn.Utils;
using System.Collections.Generic;

namespace Lumpn.ZeldaMooga
{
    public sealed class OneWayGene : ZeldaGene
    {
        public OneWayGene(ZeldaConfiguration configuration, RandomNumberGenerator random)
        {
            super(configuration);

            this.wayStart = RandomLocation(random);
            this.wayEnd = differentLocation(wayStart, random);
        }

        public OneWayGene(ZeldaConfiguration configuration, int wayStart, int wayEnd)
        {
            super(configuration);
            this.wayStart = wayStart;
            this.wayEnd = wayEnd;
        }

        public int countErrors(List<ZeldaGene> genes)
        {

            // find duplicates
            int numErrors = 0;
            for (ZeldaGene gene : genes)
            {
                if (gene is OneWayGene)
                {
                    OneWayGene other = (OneWayGene)gene;
                    if (other != this && other.equals(this)) numErrors++;
                }
            }

            return numErrors;
        }

        public OneWayGene mutate(RandomNumberGenerator random)
        {
            return new OneWayGene(getConfiguration(), random);
        }

        public void express(ZeldaDungeonBuilder builder)
        {
            builder.addDirectedTransition(wayStart, wayEnd, IdentityScript.INSTANCE);
        }

        public String toString()
        {
            return String.format("%d->%d", wayStart, wayEnd);
        }

        // transition location of one-way
        private readonly int wayStart, wayEnd;
    }
}
