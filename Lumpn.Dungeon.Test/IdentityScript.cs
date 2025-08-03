namespace Lumpn.Dungeon.Test
{
    public sealed class IdentityScript : Script
    {
        public string Name { get { return string.Empty; } }

        public State Execute(State state)
        {
            return state;
        }
    }
}
