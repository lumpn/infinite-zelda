namespace Lumpn.Dungeon2.Scripts
{
    public sealed class NoOpScript : Script
    {
        public static readonly NoOpScript instance = new NoOpScript();

        public string Name { get { return string.Empty; } }

        public ScriptResult Execute(State state, StateBuilder builder)
        {
            return ScriptResult.Pass;
        }
    }
}
