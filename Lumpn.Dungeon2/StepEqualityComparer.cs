using System.Collections.Generic;

namespace Lumpn.Dungeon
{
    public sealed class StepEqualityComparer : IEqualityComparer<Step>
    {
        public static readonly StepEqualityComparer Default = new StepEqualityComparer();

        public bool Equals(Step a, Step b)
        {
            return a.Equals(b);
        }

        public int GetHashCode(Step step)
        {
            return step.GetHashCode();
        }
    }
}
