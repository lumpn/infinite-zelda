using Lumpn.Dungeon;
using Lumpn.Dungeon.Scripts;
using System;

namespace Lumpn.ZeldaMooga
{
    public sealed class EqualsGene : ZeldaGene
    {
        private readonly string variableName, blockerName;
        private readonly int targetValue;
        private readonly int blockerStart, blockerEnd;

        public EqualsGene(ZeldaConfiguration configuration, string variableName, string blockerName, int targetValue)
            : base(configuration)
        {
            this.variableName = variableName;
            this.blockerName = blockerName;
            this.targetValue = targetValue;
            int a = configuration.RandomLocation();
            int b = configuration.RandomLocation(a);
            this.blockerStart = Math.Min(a, b);
            this.blockerEnd = Math.Max(a, b);
        }

        public override Gene Mutate()
        {
            return new EqualsGene(configuration, variableName, blockerName, targetValue);
        }

        public override void Express(CrawlerBuilder builder, VariableLookup lookup)
        {
            var script = new EqualsScript(targetValue, blockerName, variableName, lookup);
            builder.AddUndirectedTransition(blockerStart, blockerEnd, script);
        }

        public override string ToString()
        {
            return string.Format("{0} {1}--{2}", blockerName, blockerStart, blockerEnd);
        }
    }
}
