namespace Lumpn.Dungeon
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

        public ScriptResult Execute(State state, Memory buffer)
        {
            return script.Execute(state, buffer);
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
