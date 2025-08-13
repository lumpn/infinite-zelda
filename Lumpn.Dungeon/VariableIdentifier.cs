namespace Lumpn.Dungeon
{
    public sealed class VariableIdentifier
    {
        public readonly int id;
        public readonly string name;

        public VariableIdentifier(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
