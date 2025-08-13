namespace Lumpn.Dungeon.Scripts
{
    public sealed class NoOpScript : Script
    {
        public string Name { get { return string.Empty; } }

        public ScriptResult Execute(State state, Memory buffer)
        {
            return ScriptResult.Pass;
        }
    }
}
