using System;
using Lumpn.ZeldaDungeon;

namespace Lumpn.ZeldaMooga
{
    public sealed class ObstacleGene : ZeldaGene
    {
        private readonly int toolType;
        private readonly int obstacleStart, obstacleEnd;

        public ObstacleGene(ZeldaConfiguration configuration)
            : base(configuration)
        {
            this.toolType = configuration.RandomToolType();
            int a = RandomLocation();
            int b = RandomLocation(a);
            this.obstacleStart = Math.Min(a, b);
            this.obstacleEnd = Math.Max(a, b);
        }

        public override Gene Mutate()
        {
            return new ObstacleGene(Configuration);
        }

        public override void Express(ZeldaDungeonBuilder builder)
        {
            builder.AddUndirectedTransition(obstacleStart, obstacleEnd, ZeldaScripts.CreateObstacle(toolType, builder.Lookup));
        }

        public override string ToString()
        {
            return string.Format("obstacle {0} {1}--{2}", toolType, obstacleStart, obstacleEnd);
        }
    }
}
