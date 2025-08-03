using System;

namespace Lumpn.Dungeon2
{
    public struct State : IEquatable<State>
    {
        public readonly Memory memory;
        private readonly int startIndex;

        public State(Memory memory, int startIndex)
        {
            this.memory = memory;
            this.startIndex = startIndex;
        }

        public int Get(VariableIdentifier id)
        {
            return memory.Get(startIndex + id.id);
        }

        public void Set(VariableIdentifier id, int value)
        {
            memory.Set(startIndex + id.id, (byte)value);
        }

        public void CopyTo(State other)
        {
            memory.CopyTo(startIndex, other.memory, other.startIndex);
        }

        public bool Equals(State other)
        {
            return memory.AreEqual(startIndex, other.memory, other.startIndex); ;
        }

        override public int GetHashCode()
        {
            return memory.GetHashCode(startIndex);
        }

        public override string ToString()
        {
            return memory.ToString(startIndex);
        }
    }
}
