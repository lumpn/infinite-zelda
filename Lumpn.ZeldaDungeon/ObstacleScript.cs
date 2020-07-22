namespace Lumpn.ZeldaPuzzle
{
    /// permanent obstacle that can be overcome by using a tool
    public sealed class ObstacleScript : ZeldaScript
    {
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

        public void Express(DotTransitionBuilder builder)
        {
            builder.SetLabel(obstacleName);
        }

        private readonly VariableIdentifier toolIdentifier;
        private readonly string obstacleName;
    }
}
