using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace Lumpn.Dungeon.Test
{

    [TestFixture]
    public sealed partial class CrawlerTest
    {
        private const int maxSteps = 10000;
        private const string keyName = "key";
        private const string doorName = "door";

        private static readonly IdentityScript identityScript = new IdentityScript();
        private static readonly VariableAssignment emptyVariables = new VariableAssignment();

        [Test]
        public void CrawlMinimal()
        {
            // 0 - 1
            var builder = new CrawlerBuilder();
            builder.AddUndirectedTransition(0, 1, identityScript);
            var crawler = builder.Build();

            var initialState = emptyVariables.ToState(builder.Lookup);
            var terminalSteps = crawler.Crawl(new[] { initialState }, maxSteps);

            Assert.IsNotEmpty(terminalSteps);
            Assert.NotNull(crawler.DebugGetStep(0, initialState));
            Assert.NotNull(crawler.DebugGetStep(1, initialState));
            Assert.True(crawler.DebugGetStep(0, initialState).HasDistanceFromExit);
            Assert.True(crawler.DebugGetStep(1, initialState).HasDistanceFromExit);
        }

        [Test]
        public void CrawlLinear()
        {
            // 0 - 2 - 3 - 1
            var builder = new CrawlerBuilder();
            builder.AddUndirectedTransition(0, 2, identityScript);
            builder.AddUndirectedTransition(2, 3, identityScript);
            builder.AddUndirectedTransition(3, 1, identityScript);
            var crawler = builder.Build();

            var initialState = emptyVariables.ToState(builder.Lookup);
            var terminalSteps = crawler.Crawl(new[] { initialState }, maxSteps);

            Assert.NotNull(crawler.DebugGetStep(0, initialState));
            Assert.NotNull(crawler.DebugGetStep(1, initialState));
            Assert.NotNull(crawler.DebugGetStep(2, initialState));
            Assert.NotNull(crawler.DebugGetStep(3, initialState));
        }

        [Test]
        public void CrawlCyclic()
        {

            // 0 - 2 - 1
            //  \ /
            //   3
            var builder = new CrawlerBuilder();
            builder.AddUndirectedTransition(0, 2, identityScript);
            builder.AddUndirectedTransition(2, 3, identityScript);
            builder.AddUndirectedTransition(3, 0, identityScript);
            builder.AddUndirectedTransition(2, 1, identityScript);
            var crawler = builder.Build();

            var initialState = emptyVariables.ToState(builder.Lookup);
            var terminalSteps = crawler.Crawl(new[] { initialState }, maxSteps);

            Assert.NotNull(crawler.DebugGetStep(0, initialState));
            Assert.NotNull(crawler.DebugGetStep(1, initialState));
            Assert.NotNull(crawler.DebugGetStep(2, initialState));
            Assert.NotNull(crawler.DebugGetStep(3, initialState));
        }

        [Test]
        public void CrawlUnreachable()
        {
            // 0 - 2 - 1
            //
            // 3
            var builder = new CrawlerBuilder();
            builder.AddUndirectedTransition(0, 2, identityScript);
            builder.AddUndirectedTransition(2, 1, identityScript);
            builder.AddScript(3, identityScript);
            var crawler = builder.Build();

            var initialState = emptyVariables.ToState(builder.Lookup);
            crawler.Crawl(new[] { initialState }, maxSteps);

            Assert.NotNull(crawler.DebugGetStep(0, initialState));
            Assert.NotNull(crawler.DebugGetStep(1, initialState));
            Assert.NotNull(crawler.DebugGetStep(2, initialState));
            Assert.Null(crawler.DebugGetStep(3, initialState));
        }

        [Test]
        public void CrawlDeadEnd()
        {

            // 0 -> 3
            //  \
            //   2 - 1
            var builder = new CrawlerBuilder();
            builder.AddUndirectedTransition(0, 2, identityScript);
            builder.AddUndirectedTransition(2, 1, identityScript);
            builder.AddDirectedTransition(0, 3, identityScript);
            var crawler = builder.Build();

            var initialState = emptyVariables.ToState(builder.Lookup);
            crawler.Crawl(new[] { initialState }, maxSteps);

            Assert.NotNull(crawler.DebugGetStep(0, initialState));
            Assert.NotNull(crawler.DebugGetStep(1, initialState));
            Assert.NotNull(crawler.DebugGetStep(2, initialState));
            Assert.NotNull(crawler.DebugGetStep(3, initialState));
            Assert.IsEmpty(crawler.DebugGetStep(3, initialState).Successors);
            Assert.False(crawler.DebugGetStep(3, initialState).HasDistanceFromExit);
        }

        [Test]
        public void CrawlDeadCycle()
        {
            // 0 -> 2 - 3
            // |     \ /
            // 1      4
            var builder = new CrawlerBuilder();
            builder.AddUndirectedTransition(0, 1, identityScript);
            builder.AddUndirectedTransition(2, 3, identityScript);
            builder.AddUndirectedTransition(3, 4, identityScript);
            builder.AddUndirectedTransition(4, 2, identityScript);
            builder.AddDirectedTransition(0, 2, identityScript);
            var puzzle = builder.Build();

            var initialState = emptyVariables.ToState(builder.Lookup);
            puzzle.Crawl(new[] { initialState }, maxSteps);

            Assert.NotNull(puzzle.DebugGetStep(0, initialState));
            Assert.NotNull(puzzle.DebugGetStep(1, initialState));
            Assert.NotNull(puzzle.DebugGetStep(2, initialState));
            Assert.NotNull(puzzle.DebugGetStep(3, initialState));
            Assert.NotNull(puzzle.DebugGetStep(4, initialState));
            Assert.True(puzzle.DebugGetStep(0, initialState).HasDistanceFromExit);
            Assert.False(puzzle.DebugGetStep(2, initialState).HasDistanceFromExit);
            Assert.False(puzzle.DebugGetStep(3, initialState).HasDistanceFromExit);
            Assert.False(puzzle.DebugGetStep(4, initialState).HasDistanceFromExit);
        }

        [Test]
        public void CrawlKeyDoorSequential()
        {
            // 0 -d- 2 -d- 3 -d- 1
            // k     k     k
            var builder = new CrawlerBuilder();
            var lookup = builder.Lookup;

            builder.AddUndirectedTransition(0, 2, CreateDoor(lookup));
            builder.AddUndirectedTransition(2, 3, CreateDoor(lookup));
            builder.AddUndirectedTransition(3, 1, CreateDoor(lookup));
            builder.AddScript(0, CreateKey(lookup));
            builder.AddScript(2, CreateKey(lookup));
            builder.AddScript(3, CreateKey(lookup));
            var crawler = builder.Build();

            var initialState = emptyVariables.ToState(builder.Lookup);
            crawler.Crawl(new[] { initialState }, maxSteps);

            // test for exit reached
            Assert.True(crawler.DebugGetStep(0, initialState).HasDistanceFromExit);
        }

        [Test]
        public void CrawlKeyDoorParallel()
        {
            //  0 -d- 2 -d- 3 -d- 1
            // kkk
            var builder = new CrawlerBuilder();
            var lookup = builder.Lookup;

            builder.AddUndirectedTransition(0, 2, CreateDoor(lookup));
            builder.AddUndirectedTransition(2, 3, CreateDoor(lookup));
            builder.AddUndirectedTransition(3, 1, CreateDoor(lookup));
            builder.AddScript(0, CreateKey(lookup));
            builder.AddScript(0, CreateKey(lookup));
            builder.AddScript(0, CreateKey(lookup));
            var crawler = builder.Build();

            var initialState = emptyVariables.ToState(builder.Lookup);
            crawler.Crawl(new[] { initialState }, maxSteps);

            // test for exit reached
            Assert.True(crawler.DebugGetStep(0, initialState).HasDistanceFromExit);
        }

        private static KeyScript CreateKey(VariableLookup lookup)
        {
            return new KeyScript( lookup);
        }

        private static DoorScript CreateDoor(VariableLookup lookup)
        {
            return new DoorScript(lookup);
        }
    }
}