using Lumpn.ZeldaDungeon;

namespace Lumpn.ZeldaMooga
{
    public sealed class ToolGene : ZeldaGene
    {
        /// tool to overcome obstacle
        private readonly int toolType;
        private readonly int toolLocation;

        public ToolGene(ZeldaConfiguration configuration)
            : base(configuration)
        {
            this.toolType = configuration.RandomToolType();
            this.toolLocation = configuration.RandomLocation();
        }

        public override Gene Mutate()
        {
            return new ToolGene(Configuration);
        }

        public override void Express(ZeldaDungeonBuilder builder)
        {
            builder.AddScript(toolLocation, ZeldaScripts.CreateTool(toolType, builder.Lookup));
        }

        public override string ToString()
        {
            return string.Format("tool {0} at {1}", toolType, toolLocation);
        }
    }
}
