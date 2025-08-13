namespace Lumpn.Dungeon
{
    public interface Script
    {
        string Name { get; }

        ScriptResult Execute(State state, Memory buffer);
    }
}
