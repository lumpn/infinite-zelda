using System.Threading;
using NUnit.Framework;

namespace Lumpn.Profiling.Test
{
    [TestFixture]
    public sealed class ProfilerTest
    {
        [Test]
        public void TestCase()
        {
            Profiler.BeginFrame();
            {
                Profiler.BeginSample("Foo");
                Thread.Sleep(5);
                {
                    Profiler.BeginSample("Bar");
                    Thread.Sleep(5);
                    {
                        Profiler.BeginSample("Baz");
                        Thread.Sleep(10);
                        Profiler.EndSample();
                    }
                    Profiler.EndSample();
                }
                Profiler.EndSample();
            }
            Profiler.EndFrame();

            Profiler.ExportToUnity("samples.pdata");
        }
    }
}
