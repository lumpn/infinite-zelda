using System.Collections.Generic;
using System.Diagnostics;

namespace Lumpn.Profiling
{
    public sealed class Sample
    {
        private readonly string name;
        private long begin, end;

        private readonly Sample parent;
        private readonly List<Sample> children = new List<Sample>();

        public string Name { get { return name; } }
        public Sample Parent { get { return parent; } }
        public IEnumerable<Sample> Children { get { return children; } }

        public Sample(Sample parent, string name)
        {
            this.parent = parent;
            this.name = name;
        }

        public void Begin()
        {
            begin = Stopwatch.GetTimestamp();
        }

        public void End()
        {
            end = Stopwatch.GetTimestamp();
        }

        public float CalcElapsedMilliseconds(long frequency)
        {
            return TimeUtils.CalcElapsedMilliseconds(begin, end, frequency);
        }

        public void AddChild(Sample child)
        {
            children.Add(child);
        }
    }
}
