using Lumpn.Dungeon;
using Lumpn.Dungeon.Scripts;
using System;

namespace Lumpn.ZeldaMooga
{
    /// obstacle that can be overcome by using a tool
    public sealed class ObstacleGene : ZeldaGene
    {
        private readonly string toolName, obstacleName;
        private readonly int obstacleStart, obstacleEnd;

        public ObstacleGene(ZeldaConfiguration configuration, string toolName, string obstacleName)
            : base(configuration)
        {
            this.toolName = toolName;
            this.obstacleName = obstacleName;
            int a = configuration.RandomLocation();
            int b = configuration.RandomLocation(a);
            this.obstacleStart = Math.Min(a, b);
            this.obstacleEnd = Math.Max(a, b);
        }

        public override Gene Mutate()
        {
            return new ObstacleGene(configuration, toolName, obstacleName);
        }

        public override void Express(CrawlerBuilder builder, VariableLookup lookup)
        {
            var script = new GreaterThanScript(0, obstacleName, toolName, lookup);
            builder.AddUndirectedTransition(obstacleStart, obstacleEnd, script);
        }

        public override string ToString()
        {
            return string.Format("{0} {1}--{2}", obstacleName, obstacleStart, obstacleEnd);
        }
    }
}
