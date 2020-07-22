namespace Lumpn.Utils
{
    /// immutable bounds
    public class Bounds3
    {
        /// both inclusive
        public Bounds3(Int3 min, Int3 max)
        {
            this.min = min;
            this.max = max;
        }

        public bool Contains(Int3 position)
        {
            return (min <= position && position <= max);
        }

        private readonly Int3 min, max;
    }
}
