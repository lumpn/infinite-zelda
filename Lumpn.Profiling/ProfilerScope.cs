using System;

namespace Lumpn.Profiling
{
    public struct ProfilerScope : IDisposable
    {
        private readonly ProfilerImpl impl;

        public ProfilerScope(string name, ProfilerImpl impl)
        {
            this.impl = impl;
            impl.BeginSample(name);
        }

        public void Dispose()
        {
            impl.EndSample();
        }
    }
}
