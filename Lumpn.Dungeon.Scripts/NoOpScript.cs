namespace Lumpn.Dungeon.Scripts
{
    public sealed class NoOpScript : Script
    {
        public string Name { get { return string.Empty; } }

        public State Execute(State state)
        {
            return state;
        }
    }
}
