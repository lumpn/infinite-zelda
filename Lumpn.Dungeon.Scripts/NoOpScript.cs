namespace Lumpn.Dungeon.Scripts
{
    public sealed class NoOpScript : Script
    {
        public static readonly NoOpScript instance = new NoOpScript();

        public string Name { get { return string.Empty; } }

        public State Execute(State state)
        {
            return state;
        }
    }
}
