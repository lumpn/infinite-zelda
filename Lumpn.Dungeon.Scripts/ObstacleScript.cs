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

        public State Execute(State state)
        {
            // tool present?
            int numTools = state.Get(toolIdentifier, 0);
            if (numTools > 0)
            {
                return state; // pass
            }

            return null; // you shall not pass!
        }
    }
}
