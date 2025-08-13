namespace Lumpn.Dungeon
{
    public struct StateBuilder
    {
        private readonly Memory buffer;

        public StateBuilder(State sourceState, Memory buffer)
        {
            this.buffer = buffer;

            var tmpState = buffer.GetState(0);
            sourceState.CopyTo(tmpState);
        }

        public void Set(VariableIdentifier id, int value)
        {
            buffer.Set(id.id, (byte)value);
        }

        public override string ToString()
        {
            return buffer.ToString(0);
        }
    }
}
