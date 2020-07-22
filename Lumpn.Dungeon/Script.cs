namespace Lumpn.Dungeon
{
    public interface Script
    {
        string Name { get; }

        State Execute(State state);
    }
}
