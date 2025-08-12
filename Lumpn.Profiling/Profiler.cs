using System.Diagnostics;
using GoogleChromeTracingExporter = Lumpn.Profiling.GoogleChromeTracing.Exporter;
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

        public static ProfilerScope Sample(string name)
        {
            return new ProfilerScope(name, impl);
        }

        public static void AddSample(string name, long durationTicks)
        {
            impl.AddSample(name, durationTicks);
        }

        public static void ExportToGoogleChromeTracing(string filename)
        {
            var converter = new GoogleChromeTracingExporter(impl.Frames, Stopwatch.Frequency);
            converter.Export(filename);
        }

        public static void ExportToUnityProfileAnalyzer(string filename)
        {
            var converter = new UnityProfileAnalzerExporter(impl.Frames, Stopwatch.Frequency);
            converter.Export(filename);
        }
    }
}
