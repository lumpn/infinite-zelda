namespace Lumpn.Dungeon
{
    public sealed class StateBuilder
    {
        private readonly int[] variables;

        public StateBuilder(int[] variables)
        {
            this.variables = (int[])variables.Clone();
        }

        public void Set(VariableIdentifier identifier, int value)
        {
            var idx = identifier.id;
            variables[idx] = value;
        }

        public State ToState()
        {
            return new State(variables);
        }
    }
}
