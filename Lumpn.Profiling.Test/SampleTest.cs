using NUnit.Framework;
using System.Diagnostics;
using System.Threading;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace Lumpn.Profiling.Test
{
    [TestFixture]
    public sealed class SampleTest
    {
        [Test]
        public void Frequency()
        {
            var sample = new Sample(null, "Test");
            sample.Begin();
            Thread.Sleep(50);
            sample.End();
            var elapsed = sample.CalcElapsedMilliseconds(Stopwatch.Frequency);

            Assert.Greater(elapsed, 50f);
            Assert.Less(elapsed, 100f);
        }
    }
}
