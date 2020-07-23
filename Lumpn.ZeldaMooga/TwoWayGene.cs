using Lumpn.Mooga;
using Lumpn.Utils;
using System.Collections.Generic;
using System;

namespace Lumpn.ZeldaMooga
{
    public sealed class TwoWayGene : ZeldaGene
    {
        public TwoWayGene(ZeldaConfiguration configuration, RandomNumberGenerator random)
            : base(configuration)
        {
            int a = RandomLocation(random);
            int b = differentLocation(a, random);
            this.wayStart = Math.Min(a, b);
            this.wayEnd = Math.Max(a, b);
        }

        public TwoWayGene(ZeldaConfiguration configuration, int wayStart, int wayEnd)
            : base(configuration)
        {
            this.wayStart = wayStart;
            this.wayEnd = wayEnd;
        }

        public TwoWayGene Mutate(RandomNumberGenerator random)
        {
            return new TwoWayGene(getConfiguration(), random);
        }

        public int countErrors(List<ZeldaGene> genes)
        {

            // find duplicates
            int numErrors = 0;
            for (ZeldaGene gene : genes)
            {
                if (gene is TwoWayGene)
                {
                    TwoWayGene other = (TwoWayGene)gene;
                    if (other != this && other.equals(this)) numErrors++;
                }
            }

            return numErrors;
        }

        public void express(ZeldaDungeonBuilder builder)
        {
            builder.addUndirectedTransition(wayStart, wayEnd, IdentityScript.INSTANCE);
        }


        public String toString()
        {
            return String.format("%d--%d", wayStart, wayEnd);
        }

        // transition location of one-way
        private readonly int wayStart, wayEnd;
    }
}
