using Lumpn.Dungeon;

namespace Lumpn.ZeldaDungeon
{
    /// permanent obstacle that can be overcome by using a tool
    public sealed class ObstacleScript : Script
    {
        private readonly VariableIdentifier toolIdentifier;
        private readonly string obstacleName;

        public string Name { get { return obstacleName; } }

        public ObstacleScript(VariableIdentifier toolIdentifier, string obstacleName)
        {
            this.toolIdentifier = toolIdentifier;
            this.obstacleName = obstacleName;
        }

        public State Execute(State state)
        {
            // tool present?
            int numItems = state.Get(toolIdentifier, 0);
            if (numItems > 0)
            {
                return state; // pass
            }

            return null; // you shall not pass!
        }
    }
}
