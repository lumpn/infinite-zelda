namespace Lumpn.Dungeon2
{
    public struct Transition
    {
        public readonly int sourceId, destinationId;
        private readonly Script script;

        public Transition(Location source, Location destination, Script script)
        {
            this.sourceId = source.Id;
            this.destinationId = destination.Id;
            this.script = script;
        }

        public ScriptResult Execute(State state, StateBuilder builder)
        {
            return script.Execute(state, builder);
        }

        public void Express(DotBuilder builder)
        {
            builder.AddEdge(sourceId, destinationId, script.Name);
        }

        public override string ToString()
        {
            return string.Format("({0} -> {1}: {2}", sourceId, destinationId, script.Name);
        }
    }
}
