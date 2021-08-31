namespace Lumpn.Dungeon
{
    public sealed class VariableIdentifier
    {
        private readonly int id;
        private readonly string name;

        public int Id { get { return id; } }
        public string Name { get { return name; } }

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
