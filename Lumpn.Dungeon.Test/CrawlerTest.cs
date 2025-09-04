using Lumpn.Dungeon.Scripts;
using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace Lumpn.Dungeon.Test
{
    [TestFixture]
    public sealed partial class CrawlerTest
    {
        private const int maxSteps = 10000;
        private const string keyName = "key";
        private const string doorName = "door";

        private static readonly NoOpScript noOpScript = new NoOpScript();
        private static readonly VariableAssignment emptyVariables = new VariableAssignment();

        [Test]
        public void CrawlMinimal()
        {
            // 1 - 0
            var lookup = new VariableLookup();
            var builder = new CrawlerBuilder();
            builder.AddUndirectedTransition(1, 0, noOpScript);
            var crawler = builder.Build();

            var initialState = emptyVariables.ToState(lookup);
            var trace = crawler.Crawl(new[] { initialState }, maxSteps);

            Assert.NotZero(trace.CountSteps(0));
            Assert.IsTrue(trace.ContainsStep(1, initialState));
            Assert.IsTrue(trace.ContainsStep(0, initialState));
            Assert.IsTrue(trace.HasDistanceFromExit(1, initialState));
            Assert.IsTrue(trace.HasDistanceFromExit(0, initialState));
        }

        [Test]
        public void CrawlLinear()
        {
            // 1 - 2 - 3 - 0
            var lookup = new VariableLookup();
            var builder = new CrawlerBuilder();
            builder.AddUndirectedTransition(1, 2, noOpScript);
            builder.AddUndirectedTransition(2, 3, noOpScript);
            builder.AddUndirectedTransition(3, 0, noOpScript);
            var crawler = builder.Build();

            var initialState = emptyVariables.ToState(lookup);
            var trace = crawler.Crawl(new[] { initialState }, maxSteps);

            Assert.IsTrue(trace.ContainsStep(1, initialState));
            Assert.IsTrue(trace.ContainsStep(2, initialState));
            Assert.IsTrue(trace.ContainsStep(3, initialState));
            Assert.IsTrue(trace.ContainsStep(0, initialState));
        }

        [Test]
        public void CrawlCyclic()
        {

            // 1 - 2 - 0
            //  \ /
            //   3
            var lookup = new VariableLookup();
            var builder = new CrawlerBuilder();
            builder.AddUndirectedTransition(1, 2, noOpScript);
            builder.AddUndirectedTransition(2, 3, noOpScript);
            builder.AddUndirectedTransition(3, 1, noOpScript);
            builder.AddUndirectedTransition(2, 0, noOpScript);
            var crawler = builder.Build();

            var initialState = emptyVariables.ToState(lookup);
            var trace = crawler.Crawl(new[] { initialState }, maxSteps);

            Assert.IsTrue(trace.ContainsStep(1, initialState));
            Assert.IsTrue(trace.ContainsStep(2, initialState));
            Assert.IsTrue(trace.ContainsStep(3, initialState));
            Assert.IsTrue(trace.ContainsStep(0, initialState));
        }

        [Test]
        public void CrawlUnreachable()
        {
            // 1 - 2 - 0
            //
            // 3
            var lookup = new VariableLookup();
            var builder = new CrawlerBuilder();
            builder.AddUndirectedTransition(1, 2, noOpScript);
            builder.AddUndirectedTransition(2, 0, noOpScript);
            builder.AddScript(3, noOpScript);
            var crawler = builder.Build();

            var initialState = emptyVariables.ToState(lookup);
            var trace = crawler.Crawl(new[] { initialState }, maxSteps);

            Assert.IsTrue(trace.ContainsStep(1, initialState));
            Assert.IsTrue(trace.ContainsStep(2, initialState));
            Assert.IsTrue(trace.ContainsStep(0, initialState));
            Assert.IsFalse(trace.ContainsStep(3, initialState));
        }

        [Test]
        public void CrawlDeadEnd()
        {

            // 1 -> 3
            //  \
            //   2 - 0
            var lookup = new VariableLookup();
            var builder = new CrawlerBuilder();
            builder.AddUndirectedTransition(1, 2, noOpScript);
            builder.AddUndirectedTransition(2, 0, noOpScript);
            builder.AddDirectedTransition(1, 3, noOpScript);
            var crawler = builder.Build();

            var initialState = emptyVariables.ToState(lookup);
            var trace = crawler.Crawl(new[] { initialState }, maxSteps);

            Assert.IsTrue(trace.ContainsStep(1, initialState));
            Assert.IsTrue(trace.ContainsStep(2, initialState));
            Assert.IsTrue(trace.ContainsStep(3, initialState));
            Assert.IsTrue(trace.ContainsStep(0, initialState));
            Assert.Zero(trace.CountSuccessors(3, initialState));
            Assert.IsFalse(trace.HasDistanceFromExit(3, initialState));
        }

        [Test]
        public void CrawlDeadCycle()
        {
            // 1 -> 2 - 3
            // |     \ /
            // 0      4
            var lookup = new VariableLookup();
            var builder = new CrawlerBuilder();
            builder.AddUndirectedTransition(1, 0, noOpScript);
            builder.AddUndirectedTransition(2, 3, noOpScript);
            builder.AddUndirectedTransition(3, 4, noOpScript);
            builder.AddUndirectedTransition(4, 2, noOpScript);
            builder.AddDirectedTransition(1, 2, noOpScript);
            var crawler = builder.Build();

            var initialState = emptyVariables.ToState(lookup);
            var trace = crawler.Crawl(new[] { initialState }, maxSteps);

            Assert.IsTrue(trace.ContainsStep(1, initialState));
            Assert.IsTrue(trace.ContainsStep(2, initialState));
            Assert.IsTrue(trace.ContainsStep(3, initialState));
            Assert.IsTrue(trace.ContainsStep(4, initialState));
            Assert.IsTrue(trace.ContainsStep(0, initialState));
            Assert.IsTrue(trace.HasDistanceFromExit(1, initialState));
            Assert.IsFalse(trace.HasDistanceFromExit(2, initialState));
            Assert.IsFalse(trace.HasDistanceFromExit(3, initialState));
            Assert.IsFalse(trace.HasDistanceFromExit(4, initialState));
            Assert.IsTrue(trace.HasDistanceFromExit(0, initialState));
        }

        [Test]
        public void CrawlKeyDoorSequential()
        {
            // 1 -d- 2 -d- 3 -d- 0
            // k     k     k
            var lookup = new VariableLookup();
            var builder = new CrawlerBuilder();
            builder.AddUndirectedTransition(1, 2, CreateDoor(lookup));
            builder.AddUndirectedTransition(2, 3, CreateDoor(lookup));
            builder.AddUndirectedTransition(3, 0, CreateDoor(lookup));
            builder.AddScript(1, CreateKey(lookup));
            builder.AddScript(2, CreateKey(lookup));
            builder.AddScript(3, CreateKey(lookup));
            var crawler = builder.Build();

            var initialState = emptyVariables.ToState(lookup);
            var trace = crawler.Crawl(new[] { initialState }, maxSteps);

            // test for exit reached
            Assert.IsTrue(trace.HasDistanceFromExit(1, initialState));
        }

        [Test]
        public void CrawlKeyDoorParallel()
        {
            //  1 -d- 2 -d- 3 -d- 1
            // kkk
            var lookup = new VariableLookup();
            var builder = new CrawlerBuilder();
            builder.AddUndirectedTransition(1, 2, CreateDoor(lookup));
            builder.AddUndirectedTransition(2, 3, CreateDoor(lookup));
            builder.AddUndirectedTransition(3, 0, CreateDoor(lookup));
            builder.AddScript(1, CreateKey(lookup));
            builder.AddScript(1, CreateKey(lookup));
            builder.AddScript(1, CreateKey(lookup));
            var crawler = builder.Build();

            var initialState = emptyVariables.ToState(lookup);
            var trace = crawler.Crawl(new[] { initialState }, maxSteps);

            // test for exit reached
            Assert.IsTrue(trace.HasDistanceFromExit(1, initialState));
        }

        private static AcquireScript CreateKey(VariableLookup lookup)
        {
            return new AcquireScript(keyName, lookup);
        }

        private static ConsumeScript CreateDoor(VariableLookup lookup)
        {
            return new ConsumeScript(doorName, keyName, lookup);
        }
    }
}
