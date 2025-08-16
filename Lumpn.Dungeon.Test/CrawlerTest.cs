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
            var terminalSteps = crawler.Crawl(new[] { initialState }, maxSteps);

            Assert.IsNotEmpty(terminalSteps);
            Assert.NotNull(crawler.DebugGetStep(1, initialState));
            Assert.NotNull(crawler.DebugGetStep(0, initialState));
            Assert.True(crawler.DebugGetStep(1, initialState).HasDistanceFromExit);
            Assert.True(crawler.DebugGetStep(0, initialState).HasDistanceFromExit);
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
            var terminalSteps = crawler.Crawl(new[] { initialState }, maxSteps);

            Assert.NotNull(crawler.DebugGetStep(1, initialState));
            Assert.NotNull(crawler.DebugGetStep(2, initialState));
            Assert.NotNull(crawler.DebugGetStep(3, initialState));
            Assert.NotNull(crawler.DebugGetStep(0, initialState));
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
            var terminalSteps = crawler.Crawl(new[] { initialState }, maxSteps);

            Assert.NotNull(crawler.DebugGetStep(1, initialState));
            Assert.NotNull(crawler.DebugGetStep(2, initialState));
            Assert.NotNull(crawler.DebugGetStep(3, initialState));
            Assert.NotNull(crawler.DebugGetStep(0, initialState));
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
            crawler.Crawl(new[] { initialState }, maxSteps);

            Assert.NotNull(crawler.DebugGetStep(1, initialState));
            Assert.NotNull(crawler.DebugGetStep(2, initialState));
            Assert.NotNull(crawler.DebugGetStep(0, initialState));
            Assert.Null(crawler.DebugGetStep(3, initialState));
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
            crawler.Crawl(new[] { initialState }, maxSteps);

            Assert.NotNull(crawler.DebugGetStep(1, initialState));
            Assert.NotNull(crawler.DebugGetStep(2, initialState));
            Assert.NotNull(crawler.DebugGetStep(3, initialState));
            Assert.NotNull(crawler.DebugGetStep(0, initialState));
            Assert.IsEmpty(crawler.DebugGetStep(3, initialState).Successors);
            Assert.False(crawler.DebugGetStep(3, initialState).HasDistanceFromExit);
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
            var puzzle = builder.Build();

            var initialState = emptyVariables.ToState(lookup);
            puzzle.Crawl(new[] { initialState }, maxSteps);

            Assert.NotNull(puzzle.DebugGetStep(1, initialState));
            Assert.NotNull(puzzle.DebugGetStep(2, initialState));
            Assert.NotNull(puzzle.DebugGetStep(3, initialState));
            Assert.NotNull(puzzle.DebugGetStep(4, initialState));
            Assert.NotNull(puzzle.DebugGetStep(0, initialState));
            Assert.True(puzzle.DebugGetStep(1, initialState).HasDistanceFromExit);
            Assert.False(puzzle.DebugGetStep(2, initialState).HasDistanceFromExit);
            Assert.False(puzzle.DebugGetStep(3, initialState).HasDistanceFromExit);
            Assert.False(puzzle.DebugGetStep(4, initialState).HasDistanceFromExit);
            Assert.True(puzzle.DebugGetStep(0, initialState).HasDistanceFromExit);
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
            crawler.Crawl(new[] { initialState }, maxSteps);

            // test for exit reached
            Assert.True(crawler.DebugGetStep(1, initialState).HasDistanceFromExit);
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
            crawler.Crawl(new[] { initialState }, maxSteps);

            // test for exit reached
            Assert.True(crawler.DebugGetStep(1, initialState).HasDistanceFromExit);
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
