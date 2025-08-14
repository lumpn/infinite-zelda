namespace Lumpn.Dungeon2
{
    public struct StateBuilder
    {
        private readonly Memory buffer;

        public StateBuilder(Memory buffer)
        {
            this.buffer = buffer;
        }

        public void Initialize(State source)
        {
            var tmpState = buffer.GetState(0);
            source.CopyTo(tmpState);
        }

        public void Set(VariableIdentifier id, int value)
        {
            buffer.Set(id.id, (byte)value);
        }

        public State GetState()
        {
            return buffer.GetState(0);
        }

        public override string ToString()
        {
            return buffer.ToString(0);
        }
    }
}
