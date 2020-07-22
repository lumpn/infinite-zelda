namespace Lumpn.Utils
{
    public struct Pair<T>
    {
        public readonly T first, second;

        public Pair(T first, T second)
        {
            this.first = first;
            this.second = second;
        }
    }
}
