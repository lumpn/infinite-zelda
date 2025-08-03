namespace Lumpn.Dungeon2.Test
{
    public sealed class IdentityScript : Script
    {
        public string Name { get { return string.Empty; } }

        public ScriptResult Execute(State state, Memory buffer)
        {
            return ScriptResult.Pass;
        }
    }
}
