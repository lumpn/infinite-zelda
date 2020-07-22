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

        private readonly int id;
        public readonly string name;
    }
}
