using System.Diagnostics;
using UnityProfileAnalzerExporter = Lumpn.Profiling.UnityProfileAnalyzer.Exporter;

namespace Lumpn.Profiling
{
    public static class Profiler
    {
        private static readonly ProfilerImpl impl = new ProfilerImpl();

        public static void Reset()
        {
            impl.Reset();
        }

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

        public static void ExportToUnityProfileAnalyzer(string filename)
        {
            var converter = new UnityProfileAnalzerExporter(impl.Frames, Stopwatch.Frequency);
            converter.Export(filename);
        }
    }
}
