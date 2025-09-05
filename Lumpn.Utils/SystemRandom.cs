﻿using System;

namespace Lumpn.Utils
{
    public sealed class SystemRandom : RandomNumberGenerator
    {
        private readonly Random random;

        public SystemRandom(int seed)
        {
            random = new Random(seed);
        }

        public int NextInt(int maxExclusive)
        {
            return random.Next(maxExclusive);
        }

        public double NextDouble()
        {
            return random.NextDouble();
        }
    }
}
