namespace Lumpn.Dungeon.Scripts
{
    /// permanent obstacle that can be overcome by using a tool
    public sealed class ObstacleScript : Script
    {
        private readonly string obstacleName;
        private readonly VariableIdentifier toolIdentifier;

        public string Name { get { return obstacleName; } }

        public ObstacleScript(string obstacleName, string toolName, VariableLookup lookup)
        {
            this.obstacleName = obstacleName;
            this.toolIdentifier = lookup.Resolve(toolName);
        }

        public ScriptResult Execute(State state, Memory buffer)
        {
            // tool present?
            int numTools = state.Get(toolIdentifier);
            if (numTools > 0)
            {
                return ScriptResult.Pass;
            }

            return ScriptResult.Fail;
        }
    }
}
