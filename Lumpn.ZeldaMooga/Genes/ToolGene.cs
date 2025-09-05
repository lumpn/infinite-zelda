using Lumpn.Dungeon;
using Lumpn.Dungeon.Scripts;

namespace Lumpn.ZeldaMooga
{
    /// tool to overcome obstacle
    public sealed class ToolGene : ZeldaGene
    {
        private readonly string toolName;
        private readonly int toolLocation;

        public ToolGene(ZeldaConfiguration configuration, string toolName)
            : base(configuration)
        {
            this.toolName = toolName;
            this.toolLocation = configuration.RandomLocation();
        }

        public override Gene Mutate()
        {
            return new ToolGene(configuration, toolName);
        }

        public override void Express(CrawlerBuilder builder, VariableLookup lookup)
        {
            var toolScript = new AcquireScript(toolName, lookup);
            builder.AddScript(toolLocation, toolScript);
        }

        public override string ToString()
        {
            return string.Format("{0} at {1}", toolName, toolLocation);
        }
    }
}
