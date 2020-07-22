using System;

namespace Lumpn.Utils
{
    public static class Debug
    {
        public static void Fail()
        {
            throw new InvalidOperationException();
        }

        public static void Fail(string message)
        {
            throw new InvalidOperationException(message);
        }

        public static void Assert(bool condition)
        {
            if (!condition) throw new InvalidOperationException();
        }

        public static void Assert(bool condition, string message)
        {
            if (!condition) throw new InvalidOperationException(message);
        }
    }
}
