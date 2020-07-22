namespace Lumpn.ZeldaPuzzle
{
    public sealed class VariableIdentifier
    {
        public VariableIdentifier(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public override string ToString()
        {
            return string.Format("{0}", name);
        }

        public int Id { get { return id; } }
        public string Name { get { return name; } }

        private readonly int id;
        private readonly string name;
    }
}
