using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Lumpn.ZeldaDungeon.Test;
using Microsoft.VSDiagnostics;

namespace Lumpn.ZeldaDungeon.Benchmark
{
    [CPUUsageDiagnoser]
    public class Program
    {
        [Benchmark]
        public void Jabu()
        {
            var fixture = new OracleAges();
            fixture.JabuJabuBelly();
        }

        [Benchmark]
        public void Jabu2()
        {
            var fixture = new OracleAges2();
            fixture.JabuJabuBelly();
        }

        public static void Main(string[] args)
        {
            var inst = new Program();
            inst.Jabu();
            inst.Jabu2();
            //var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }
}
