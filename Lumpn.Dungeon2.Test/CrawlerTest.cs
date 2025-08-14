using Lumpn.Dungeon2.Scripts;
using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace Lumpn.Dungeon2.Test
{
    [TestFixture]
    public sealed partial class CrawlerTest
    {
        private const int maxSteps = 10000;
        private const string keyName = "key";
        private const string doorName = "door";

        private static readonly NoOpScript noOpScript = new NoOpScript();

        [Test]
        public void CrawlMinimal()
        {
            // 1 - 0
            var builder = new CrawlerBuilder();
            builder.AddUndirectedTransition(1, 0, noOpScript);
            var crawler = builder.Build();

            var terminalSteps = crawler.Crawl(maxSteps);
            var deadEndSteps = crawler.GetDeadEnds();

            Assert.IsNotEmpty(terminalSteps);
            Assert.IsEmpty(deadEndSteps);
            Assert.AreEqual(1, terminalSteps.Count);
            //Assert.NotNull(crawler.DebugGetStep(0, initialState));
            //Assert.NotNull(crawler.DebugGetStep(1, initialState));
            //Assert.True(crawler.DebugGetStep(0, initialState).HasDistanceFromExit);
            //Assert.True(crawler.DebugGetStep(1, initialState).HasDistanceFromExit);
        }

        [Test]
        public void CrawlUnreachable()
        {
            // 1
            //
            // 0
            var builder = new CrawlerBuilder();
            builder.AddScript(1, noOpScript);
            builder.AddScript(0, noOpScript);
            var crawler = builder.Build();

            var terminalSteps = crawler.Crawl(maxSteps);
            var deadEndSteps = crawler.GetDeadEnds();

            Assert.IsEmpty(terminalSteps);
            Assert.IsNotEmpty(deadEndSteps);
            //Assert.NotNull(crawler.DebugGetStep(0, initialState));
            //Assert.NotNull(crawler.DebugGetStep(1, initialState));
            //Assert.True(crawler.DebugGetStep(0, initialState).HasDistanceFromExit);
            //Assert.True(crawler.DebugGetStep(1, initialState).HasDistanceFromExit);
        }

        [Test]
        public void CrawlLinear()
        {
            // 1 - 2 - 3 - 0
            var builder = new CrawlerBuilder();
            builder.AddUndirectedTransition(1, 2, noOpScript);
            builder.AddUndirectedTransition(2, 3, noOpScript);
            builder.AddUndirectedTransition(3, 0, noOpScript);
            var crawler = builder.Build();

            var terminalSteps = crawler.Crawl(maxSteps);
            var deadEndSteps = crawler.GetDeadEnds();

            Assert.IsNotEmpty(terminalSteps);
            Assert.IsEmpty(deadEndSteps);
            Assert.AreEqual(1, terminalSteps.Count);
            //Assert.NotNull(crawler.DebugGetStep(0, initialState));
            //Assert.NotNull(crawler.DebugGetStep(1, initialState));
            //Assert.NotNull(crawler.DebugGetStep(2, initialState));
            //Assert.NotNull(crawler.DebugGetStep(3, initialState));
        }

        [Test]
        public void CrawlCyclic()
        {
            // 1 - 2 - 0
            //  \ /
            //   3
            var builder = new CrawlerBuilder();
            builder.AddUndirectedTransition(1, 2, noOpScript);
            builder.AddUndirectedTransition(2, 3, noOpScript);
            builder.AddUndirectedTransition(3, 1, noOpScript);
            builder.AddUndirectedTransition(2, 0, noOpScript);
            var crawler = builder.Build();

            var terminalSteps = crawler.Crawl(maxSteps);
            var deadEndSteps = crawler.GetDeadEnds();

            Assert.IsNotEmpty(terminalSteps);
            Assert.IsEmpty(deadEndSteps);
            Assert.AreEqual(1, terminalSteps.Count);
            //Assert.NotNull(crawler.DebugGetStep(0, initialState));
            //Assert.NotNull(crawler.DebugGetStep(1, initialState));
            //Assert.NotNull(crawler.DebugGetStep(2, initialState));
            //Assert.NotNull(crawler.DebugGetStep(3, initialState));
        }

        [Test]
        public void CrawlUnreachableBranch()
        {
            // 1 - 2 - 0
            //
            // 3
            var builder = new CrawlerBuilder();
            builder.AddUndirectedTransition(1, 2, noOpScript);
            builder.AddUndirectedTransition(2, 0, noOpScript);
            builder.AddScript(3, noOpScript);
            var crawler = builder.Build();

            var terminalSteps = crawler.Crawl(maxSteps);
            var deadEndSteps = crawler.GetDeadEnds();

            Assert.IsNotEmpty(terminalSteps);
            Assert.IsEmpty(deadEndSteps);
            Assert.AreEqual(1, terminalSteps.Count);
            //Assert.NotNull(crawler.DebugGetStep(0, initialState));
            //Assert.NotNull(crawler.DebugGetStep(1, initialState));
            //Assert.NotNull(crawler.DebugGetStep(2, initialState));
            //Assert.Null(crawler.DebugGetStep(3, initialState));
        }

        [Test]
        public void CrawlDeadEnd()
        {

            // 1 -> 3
            //  \
            //   2 - 0
            var builder = new CrawlerBuilder();
            builder.AddUndirectedTransition(1, 2, noOpScript);
            builder.AddUndirectedTransition(2, 0, noOpScript);
            builder.AddDirectedTransition(1, 3, noOpScript);
            var crawler = builder.Build();

            var terminalSteps = crawler.Crawl(maxSteps);
            var deadEndSteps = crawler.GetDeadEnds();

            Assert.IsNotEmpty(terminalSteps);
            Assert.IsNotEmpty(deadEndSteps);
            Assert.AreEqual(1, terminalSteps.Count);
            //Assert.NotNull(crawler.DebugGetStep(0, initialState));
            //Assert.NotNull(crawler.DebugGetStep(1, initialState));
            //Assert.NotNull(crawler.DebugGetStep(2, initialState));
            //Assert.NotNull(crawler.DebugGetStep(3, initialState));
            //Assert.IsEmpty(crawler.DebugGetStep(3, initialState).Successors);
            //Assert.False(crawler.DebugGetStep(3, initialState).HasDistanceFromExit);
        }

        [Test]
        public void CrawlDeadCycle()
        {
            // 1 -> 2 - 3
            // |     \ /
            // 0      4
            var builder = new CrawlerBuilder();
            builder.AddDirectedTransition(1, 2, noOpScript);
            builder.AddUndirectedTransition(2, 3, noOpScript);
            builder.AddUndirectedTransition(3, 4, noOpScript);
            builder.AddUndirectedTransition(4, 2, noOpScript);
            builder.AddUndirectedTransition(1, 0, noOpScript);
            var crawler = builder.Build();

            var terminalSteps = crawler.Crawl(maxSteps);
            var deadEndSteps = crawler.GetDeadEnds();

            Assert.IsNotEmpty(terminalSteps);
            Assert.IsNotEmpty(deadEndSteps);
            Assert.AreEqual(1, terminalSteps.Count);
            //Assert.NotNull(puzzle.DebugGetStep(0, initialState));
            //Assert.NotNull(puzzle.DebugGetStep(1, initialState));
            //Assert.NotNull(puzzle.DebugGetStep(2, initialState));
            //Assert.NotNull(puzzle.DebugGetStep(3, initialState));
            //Assert.NotNull(puzzle.DebugGetStep(4, initialState));
            //Assert.True(puzzle.DebugGetStep(0, initialState).HasDistanceFromExit);
            //Assert.False(puzzle.DebugGetStep(2, initialState).HasDistanceFromExit);
            //Assert.False(puzzle.DebugGetStep(3, initialState).HasDistanceFromExit);
            //Assert.False(puzzle.DebugGetStep(4, initialState).HasDistanceFromExit);
        }

        [Test]
        public void CrawlKeyDoorSequential()
        {
            // 1 -d- 2 -d- 3 -d- 0
            // k     k     k
            var builder = new CrawlerBuilder();
            var lookup = builder.Lookup;

            builder.AddUndirectedTransition(1, 2, CreateDoor(lookup));
            builder.AddUndirectedTransition(2, 3, CreateDoor(lookup));
            builder.AddUndirectedTransition(3, 0, CreateDoor(lookup));
            builder.AddScript(1, CreateKey(lookup));
            builder.AddScript(2, CreateKey(lookup));
            builder.AddScript(3, CreateKey(lookup));
            var crawler = builder.Build();

            var terminalSteps = crawler.Crawl(maxSteps);
            var deadEndSteps = crawler.GetDeadEnds();

            Assert.IsNotEmpty(terminalSteps);
            Assert.IsEmpty(deadEndSteps);
            Assert.AreEqual(1, terminalSteps.Count);
        }

        [Test]
        public void CrawlKeyDoorParallel()
        {
            //  1 -d- 2 -d- 3 -d- 0
            // kkk
            var builder = new CrawlerBuilder();
            var lookup = builder.Lookup;

            builder.AddUndirectedTransition(1, 2, CreateDoor(lookup));
            builder.AddUndirectedTransition(2, 3, CreateDoor(lookup));
            builder.AddUndirectedTransition(3, 0, CreateDoor(lookup));
            builder.AddScript(1, CreateKey(lookup));
            builder.AddScript(1, CreateKey(lookup));
            builder.AddScript(1, CreateKey(lookup));
            var crawler = builder.Build();

            var terminalSteps = crawler.Crawl(maxSteps);
            var deadEndSteps = crawler.GetDeadEnds();

            Assert.IsNotEmpty(terminalSteps);
            Assert.IsEmpty(deadEndSteps);
            Assert.AreEqual(1, terminalSteps.Count);
        }

        [Test]
        public void CrawlKeyDoorPotentialDeadEnd()
        {
            //  0 -d- 1 -d- 2
            //        k
            var builder = new CrawlerBuilder();
            var lookup = builder.Lookup;

            builder.AddUndirectedTransition(0, 1, CreateDoor(lookup));
            builder.AddUndirectedTransition(1, 2, CreateDoor(lookup));
            builder.AddScript(1, CreateKey(lookup));
            var crawler = builder.Build();

            var terminalSteps = crawler.Crawl(maxSteps);
            var deadEndSteps = crawler.GetDeadEnds();

            Assert.IsNotEmpty(terminalSteps);
            Assert.IsNotEmpty(deadEndSteps);
            Assert.AreEqual(1, terminalSteps.Count);
        }

        [Test]
        public void CrawlKeyDoorMultipleEndings()
        {
            //  k     k
            //  1 -d- 2
            //   \
            //    d
            //     \
            //      0
            var builder = new CrawlerBuilder();
            var lookup = builder.Lookup;

            builder.AddUndirectedTransition(0, 1, CreateDoor(lookup));
            builder.AddUndirectedTransition(1, 2, CreateDoor(lookup));
            builder.AddScript(1, CreateKey(lookup));
            builder.AddScript(2, CreateKey(lookup));
            var crawler = builder.Build();

            var terminalSteps = crawler.Crawl(maxSteps);
            var deadEndSteps = crawler.GetDeadEnds();
            crawler.PrintTrace(new DotBuilder());

            Assert.IsNotEmpty(terminalSteps);
            Assert.IsEmpty(deadEndSteps);
            Assert.AreEqual(2, terminalSteps.Count);
        }

        private static ItemScript CreateKey(VariableLookup lookup)
        {
            return new ItemScript(keyName, lookup);
        }

        private static DoorScript CreateDoor(VariableLookup lookup)
        {
            return new DoorScript(keyName, doorName, lookup);
        }
    }
}
