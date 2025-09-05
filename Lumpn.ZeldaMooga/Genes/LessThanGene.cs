using Lumpn.Dungeon;
using Lumpn.Dungeon.Scripts;
using System;

namespace Lumpn.ZeldaMooga
{
    public sealed class LessThanGene : ZeldaGene
    {
        private readonly string blockerName, variableName;
        private readonly int thresholdValue;
        private readonly int blockerStart, blockerEnd;

        public LessThanGene(ZeldaConfiguration configuration, string blockerName, string variableName, int thresholdValue)
            : base(configuration)
        {
            this.variableName = variableName;
            this.thresholdValue = thresholdValue;
            int a = configuration.RandomLocation();
            int b = configuration.RandomLocation(a);
            this.blockerStart = Math.Min(a, b);
            this.blockerEnd = Math.Max(a, b);
        }

        public override Gene Mutate()
        {
            return new LessThanGene(configuration, blockerName, variableName, thresholdValue);
        }

        public override void Express(CrawlerBuilder builder, VariableLookup lookup)
        {
            var script = new LessThanScript(thresholdValue, blockerName, variableName, lookup);
            builder.AddUndirectedTransition(blockerStart, blockerEnd, script);
        }

        public override string ToString()
        {
            return string.Format("{0} {1}--{2}", blockerName, blockerStart, blockerEnd);
        }
    }
}
