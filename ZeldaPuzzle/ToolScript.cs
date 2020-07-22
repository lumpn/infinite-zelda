namespace Lumpn.ZeldaPuzzle
{
    /// unique tool that can only be picked up once and never lost
    /// therefore we don't need to store its state
    public sealed class ToolScript : ZeldaScript
    {
        public ToolScript(VariableIdentifier toolIdentifier)
        {
            this.toolIdentifier = toolIdentifier;
        }

        public State Execute(State state)
        {
            // already acquired?
            int numTools = state.Get(toolIdentifier, 0);
            if (numTools > 0) return state;

            // acquire tool
            var builder = state.ToStateBuilder();
            builder.Set(toolIdentifier, 1);
            return builder.ToState();
        }

        public void Express(DotTransitionBuilder builder)
        {
            builder.SetLabel(toolIdentifier.name);
        }

        private readonly VariableIdentifier toolIdentifier;
    }
}
