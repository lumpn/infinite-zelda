namespace Lumpn.Dungeon.Scripts
{
    /// an AND blocker
    public sealed class AndScript : Script
    {
        private readonly Script a, b;

        public string Name { get { return $"{a.Name} AND {b.Name}"; } }

        public AndScript(Script a, Script b)
        {
            this.a = a;
            this.b = b;
        }

        public State Execute(State state)
        {
            var tmpState = a.Execute(state);
            if (tmpState == null)
            {
                return null;
            }
            return b.Execute(tmpState);
        }
    }
}
