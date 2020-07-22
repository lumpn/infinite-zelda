using Lumpn.Dungeon;

namespace Lumpn.ZeldaDungeon
{
    public sealed class IdentityScript : Script
    {
        public static readonly IdentityScript Default = new IdentityScript();

        public string Name { get { return string.Empty; } }

        public State Execute(State state)
        {
            return state;
        }
    }
}
