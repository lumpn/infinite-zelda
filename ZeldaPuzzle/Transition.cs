namespace Lumpn.ZeldaPuzzle
{
    public sealed class Transition
    {
        public Transition(Location source, Location destination, ZeldaScript script)
        {
            this.source = source;
            this.destination = destination;
            this.script = script;
        }

        public State Execute(State state)
        {
            return script.Execute(state);
        }

        public void Express(DotTransitionBuilder builder)
        {
            builder.SetEdge(source.id, destination.id);
            script.Express(builder);
        }

        public readonly Location source, destination;
        private readonly ZeldaScript script;
    }
}
