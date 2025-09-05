﻿namespace Lumpn.Utils
{
    public interface RandomNumberGenerator
    {
        /// random value between 0 (inclusive) and max (exclusive)
        int NextInt(int maxExclusive);

        /// random value between 0 (inclusive) and 1 (exclusive)
        double NextDouble();
    }
}
