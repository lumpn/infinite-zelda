using Lumpn.Dungeon;
using Lumpn.Dungeon.Scripts;
using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace Lumpn.ZeldaDungeon.Test
{
    [TestFixture]
    public sealed class ZeldaDungeonTest
    {
        private const int maxSteps = 10000;
        private const int redPistonState = 0;
        private const int bluePistonState = 1;

        private static readonly VariableAssignment emptyVariables = new VariableAssignment();

        [Test]
        public void CrawlMinimal()
        {
            // 1 - 0
            var builder = new ZeldaDungeonBuilder();
            builder.AddUndirectedTransition(1, 0, NoOpScript.instance);
            var crawler = builder.Build();

            var initialState = emptyVariables.ToState(builder.Lookup);
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
            var builder = new ZeldaDungeonBuilder();
            builder.AddUndirectedTransition(1, 2, NoOpScript.instance);
            builder.AddUndirectedTransition(2, 3, NoOpScript.instance);
            builder.AddUndirectedTransition(3, 0, NoOpScript.instance);
            var crawler = builder.Build();

            var initialState = emptyVariables.ToState(builder.Lookup);
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
            var builder = new ZeldaDungeonBuilder();
            builder.AddUndirectedTransition(1, 2, NoOpScript.instance);
            builder.AddUndirectedTransition(2, 3, NoOpScript.instance);
            builder.AddUndirectedTransition(3, 1, NoOpScript.instance);
            builder.AddUndirectedTransition(2, 0, NoOpScript.instance);
            var crawler = builder.Build();

            var initialState = emptyVariables.ToState(builder.Lookup);
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
            var builder = new ZeldaDungeonBuilder();
            builder.AddUndirectedTransition(1, 2, NoOpScript.instance);
            builder.AddUndirectedTransition(2, 0, NoOpScript.instance);
            builder.AddScript(3, NoOpScript.instance);
            var crawler = builder.Build();

            var initialState = emptyVariables.ToState(builder.Lookup);
            var trace = crawler.Crawl(new[] { initialState }, maxSteps);

            Assert.IsTrue(trace.ContainsStep(1, initialState));
            Assert.IsTrue(trace.ContainsStep(2, initialState));
            Assert.IsTrue(trace.ContainsStep(0, initialState));
            Assert.False(trace.ContainsStep(3, initialState));
        }

        [Test]
        public void CrawlDeadEnd()
        {
            // 1 -> 3
            //  \
            //   2 - 0
            var builder = new ZeldaDungeonBuilder();
            builder.AddUndirectedTransition(1, 2, NoOpScript.instance);
            builder.AddUndirectedTransition(2, 0, NoOpScript.instance);
            builder.AddDirectedTransition(1, 3, NoOpScript.instance);
            var crawler = builder.Build();

            var initialState = emptyVariables.ToState(builder.Lookup);
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
            var builder = new ZeldaDungeonBuilder();
            builder.AddUndirectedTransition(1, 0, NoOpScript.instance);
            builder.AddUndirectedTransition(2, 3, NoOpScript.instance);
            builder.AddUndirectedTransition(3, 4, NoOpScript.instance);
            builder.AddUndirectedTransition(4, 2, NoOpScript.instance);
            builder.AddDirectedTransition(1, 2, NoOpScript.instance);
            var crawler = builder.Build();

            var initialState = emptyVariables.ToState(builder.Lookup);
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
        public void CrawlKeyDoorLinear()
        {
            // 1 -d- 2 -d- 3 -d- 0
            // k     k     k
            var builder = new ZeldaDungeonBuilder();
            var lookup = builder.Lookup;

            builder.AddUndirectedTransition(1, 2, ZeldaScripts.CreateDoor(0, lookup));
            builder.AddUndirectedTransition(2, 3, ZeldaScripts.CreateDoor(0, lookup));
            builder.AddUndirectedTransition(3, 0, ZeldaScripts.CreateDoor(0, lookup));
            builder.AddScript(1, ZeldaScripts.CreateKey(0, lookup));
            builder.AddScript(2, ZeldaScripts.CreateKey(0, lookup));
            builder.AddScript(3, ZeldaScripts.CreateKey(0, lookup));
            var crawler = builder.Build();

            var initialState = emptyVariables.ToState(builder.Lookup);
            var trace = crawler.Crawl(new[] { initialState }, maxSteps);

            // test for exit reached
            Assert.IsTrue(trace.HasDistanceFromExit(1, initialState));
        }

        [Test]
        public void CrawlKeyDoorBatch()
        {
            //  1 -d- 2 -d- 3 -d- 0
            // kkk
            var builder = new ZeldaDungeonBuilder();
            var lookup = builder.Lookup;

            builder.AddUndirectedTransition(1, 2, ZeldaScripts.CreateDoor(0, lookup));
            builder.AddUndirectedTransition(2, 3, ZeldaScripts.CreateDoor(0, lookup));
            builder.AddUndirectedTransition(3, 0, ZeldaScripts.CreateDoor(0, lookup));
            builder.AddScript(1, ZeldaScripts.CreateKey(0, lookup));
            builder.AddScript(1, ZeldaScripts.CreateKey(0, lookup));
            builder.AddScript(1, ZeldaScripts.CreateKey(0, lookup));
            var crawler = builder.Build();

            var initialState = emptyVariables.ToState(builder.Lookup);
            var trace = crawler.Crawl(new[] { initialState }, maxSteps);

            // test for exit reached
            Assert.IsTrue(trace.HasDistanceFromExit(1, initialState));
        }

        [Test]
        public void CrawlGenome()
        {
            // 1--7, 1--9, 2--3, Key: 3, Key: 7, Key: 8, Key: 9, Door: 4--6, Door: 5--6, Door: 5--8, Door: 8--9, Switch: 4, Piston: 0--7, Piston: 2--6

            var builder = new ZeldaDungeonBuilder();
            var lookup = builder.Lookup;

            // 1--7, 1--9, 2--3
            builder.AddUndirectedTransition(1, 7, NoOpScript.instance);
            builder.AddUndirectedTransition(1, 9, NoOpScript.instance);
            builder.AddUndirectedTransition(2, 3, NoOpScript.instance);

            // Key: 3, Key: 7, Key: 8, Key: 9
            builder.AddScript(3, ZeldaScripts.CreateKey(0, lookup));
            builder.AddScript(7, ZeldaScripts.CreateKey(0, lookup));
            builder.AddScript(8, ZeldaScripts.CreateKey(0, lookup));
            builder.AddScript(9, ZeldaScripts.CreateKey(0, lookup));

            // Door: 4--6, Door: 5--6, Door: 5--8, Door: 8--9
            builder.AddUndirectedTransition(4, 6, ZeldaScripts.CreateDoor(0, lookup));
            builder.AddUndirectedTransition(5, 6, ZeldaScripts.CreateDoor(0, lookup));
            builder.AddUndirectedTransition(5, 8, ZeldaScripts.CreateDoor(0, lookup));
            builder.AddUndirectedTransition(8, 9, ZeldaScripts.CreateDoor(0, lookup));

            // Switch: 4
            builder.AddScript(4, ZeldaScripts.CreateColorSwitch(0, lookup));

            // Piston: 0--7, Piston: 2--6
            builder.AddUndirectedTransition(0, 7, ZeldaScripts.CreateColorPiston(0, bluePistonState, lookup));
            builder.AddUndirectedTransition(2, 6, ZeldaScripts.CreateColorPiston(0, redPistonState, lookup));

            var crawler = builder.Build();
            var dot = new DotBuilder();
            crawler.Express(dot);

            var initialState = emptyVariables.ToState(builder.Lookup);
            var trace = crawler.Crawl(new[] { initialState }, 10000);

            // test for exit reached
            Assert.IsTrue(trace.HasDistanceFromExit(1, initialState));
        }
    }
}
