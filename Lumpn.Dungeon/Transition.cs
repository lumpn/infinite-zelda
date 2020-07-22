namespace Lumpn.Dungeon
{
    public sealed class Transition
    {
        private readonly Location source, destination;
        private readonly Script script;

        public Location Source { get { return source; } }
        public Location Destination { get { return destination; } }

        public Transition(Location source, Location destination, Script script)
        {
            this.source = source;
            this.destination = destination;
            this.script = script;
        }

        public State Execute(State state)
        {
            return script.Execute(state);
        }

        public void Express(DotBuilder builder)
        {
            builder.AddEdge(source.Id, destination.Id, script.Name);
        }

    }
}
