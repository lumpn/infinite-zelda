using System;
using System.Text;

namespace Lumpn.Dungeon2
{
    public sealed class Memory
    {
        private readonly int stateSize;
        private int freeIndex;
        private byte[] data;

        public Memory(int stateSize, int numInitialStates)
        {
            this.stateSize = stateSize;
            freeIndex = 0;
            data = new byte[stateSize * numInitialStates];
        }

        public State Allocate()
        {
            var minSize = freeIndex + stateSize;
            if (minSize > data.Length)
            {
                var requestedSize = data.Length * 2;
                while (minSize > requestedSize)
                {
                    requestedSize *= 2;
                }
                Array.Resize(ref data, requestedSize);
            }
            var startIndex = freeIndex;
            freeIndex += stateSize;
            return new State(this, startIndex);
        }

        public State GetState(int stateIdx)
        {
            return new State(this, stateIdx * stateSize);
        }

        public byte Get(int idx)
        {
            return data[idx];
        }

        public void Set(int idx, byte value)
        {
            data[idx] = value;
        }

        public void CopyTo(int startIndex, Memory destinationMemory, int destinationStartIndex)
        {
            Array.Copy(data, startIndex, destinationMemory.data, destinationStartIndex, stateSize);
        }

        public bool AreEqual(int startIndex, Memory otherMemory, int otherStartIndex)
        {
            var otherData = otherMemory.data;
            if (otherData == data && otherStartIndex == startIndex)
            {
                return true;
            }

            for (int i = 0; i < stateSize; i++)
            {
                if (data[startIndex + i] != otherData[otherStartIndex + i])
                {
                    return false;
                }
            }
            return true;
        }

        public int GetHashCode(int startIndex)
        {
            unchecked
            {
                var result = 0;
                for (int i = 0; i < stateSize; i++)
                {
                    result = (result * 31) ^ data[startIndex + i];
                }
                return result;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder("[");
            if (stateSize < data.Length)
            {
                sb.Append(data[0]);
                for (int i = 1; i < freeIndex; i++)
                {
                    sb.Append(", ");
                    sb.Append(data[i]);
                }
            }
            sb.Append("]");
            return sb.ToString();
        }

        public string ToString(int startIndex)
        {
            var sb = new StringBuilder("[");
            if (startIndex >= 0 && startIndex + stateSize < data.Length)
            {
                sb.Append(data[startIndex]);
                for (int i = 1; i < stateSize; i++)
                {
                    sb.Append(", ");
                    sb.Append(data[startIndex + i]);
                }
            }
            sb.Append("]");
            return sb.ToString();
        }
    }
}
