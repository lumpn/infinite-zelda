using System.Diagnostics;

namespace Lumpn.Profiling
{
    public static class Profiler
    {
        private static readonly ProfilerImpl impl = new ProfilerImpl();

        public static void BeginFrame()
        {
            impl.BeginFrame();
        }

        public static void EndFrame()
        {
            impl.EndFrame();
        }

        public static void BeginSample(string name)
        {
            impl.BeginSample(name);
        }

        public static void EndSample()
        {
            impl.EndSample();
        }

        public static void ExportToUnity(string filename)
        {
            var converter = new Lumpn.Profiling.Unity.Converter();
            converter.Convert(impl.Frames, Stopwatch.Frequency);
            converter.Export(filename);
        }
    }
}
