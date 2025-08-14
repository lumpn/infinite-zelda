namespace Lumpn.Dungeon2
{
    public interface Script
    {
        string Name { get; }

        ScriptResult Execute(State state, StateBuilder builder);
    }
}
