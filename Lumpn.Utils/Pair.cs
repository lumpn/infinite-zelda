namespace Lumpn.Utils
{
    public struct Pair<T>
    {
        private readonly T first, second;

        public T First { get { return first; } }
        public T Second { get { return second; } }

        public Pair(T first, T second)
        {
            this.first = first;
            this.second = second;
        }
    }
}
