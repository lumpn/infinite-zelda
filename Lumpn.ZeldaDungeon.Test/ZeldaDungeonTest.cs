﻿using Lumpn.Dungeon;
using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace Lumpn.ZeldaDungeon.Test
{
    [TestFixture]
    public sealed class ZeldaDungeonTest
    {
        private const int maxSteps = 10000;

        private static readonly VariableAssignment emptyVariables = new VariableAssignment();

        [Test]
        public void CrawlMinimal()
        {
            // 0 - 1
            var builder = new ZeldaDungeonBuilder();
            builder.AddUndirectedTransition(0, 1, IdentityScript.Default);
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
            var builder = new ZeldaDungeonBuilder();
            builder.AddUndirectedTransition(0, 2, IdentityScript.Default);
            builder.AddUndirectedTransition(2, 3, IdentityScript.Default);
            builder.AddUndirectedTransition(3, 1, IdentityScript.Default);
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
            var builder = new ZeldaDungeonBuilder();
            builder.AddUndirectedTransition(0, 2, IdentityScript.Default);
            builder.AddUndirectedTransition(2, 3, IdentityScript.Default);
            builder.AddUndirectedTransition(3, 0, IdentityScript.Default);
            builder.AddUndirectedTransition(2, 1, IdentityScript.Default);
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
            var builder = new ZeldaDungeonBuilder();
            builder.AddUndirectedTransition(0, 2, IdentityScript.Default);
            builder.AddUndirectedTransition(2, 1, IdentityScript.Default);
            builder.AddScript(3, IdentityScript.Default);
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
            var builder = new ZeldaDungeonBuilder();
            builder.AddUndirectedTransition(0, 2, IdentityScript.Default);
            builder.AddUndirectedTransition(2, 1, IdentityScript.Default);
            builder.AddDirectedTransition(0, 3, IdentityScript.Default);
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
            var builder = new ZeldaDungeonBuilder();
            builder.AddUndirectedTransition(0, 1, IdentityScript.Default);
            builder.AddUndirectedTransition(2, 3, IdentityScript.Default);
            builder.AddUndirectedTransition(3, 4, IdentityScript.Default);
            builder.AddUndirectedTransition(4, 2, IdentityScript.Default);
            builder.AddDirectedTransition(0, 2, IdentityScript.Default);
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
        public void testCrawlKeyDoorLinear()
        {
            // 0 -d- 2 -d- 3 -d- 1
            // k     k     k
            var builder = new ZeldaDungeonBuilder();
            var lookup = builder.Lookup;

            builder.AddUndirectedTransition(0, 2, ZeldaScripts.CreateDoor(0, lookup));
            builder.AddUndirectedTransition(2, 3, ZeldaScripts.CreateDoor(0, lookup));
            builder.AddUndirectedTransition(3, 1, ZeldaScripts.CreateDoor(0, lookup));
            builder.AddScript(0, ZeldaScripts.CreateKey(0, lookup));
            builder.AddScript(2, ZeldaScripts.CreateKey(0, lookup));
            builder.AddScript(3, ZeldaScripts.CreateKey(0, lookup));
            var crawler = builder.Build();

            var initialState = emptyVariables.ToState(builder.Lookup);
            crawler.Crawl(new[] { initialState }, maxSteps);

            // test for exit reached
            Assert.True(crawler.DebugGetStep(0, initialState).HasDistanceFromExit);
        }

        [Test]
        public void CrawlKeyDoorBatch()
        {
            //  0 -d- 2 -d- 3 -d- 1
            // kkk
            var builder = new ZeldaDungeonBuilder();
            var lookup = builder.Lookup;

            builder.AddUndirectedTransition(0, 2, ZeldaScripts.CreateDoor(0, lookup));
            builder.AddUndirectedTransition(2, 3, ZeldaScripts.CreateDoor(0, lookup));
            builder.AddUndirectedTransition(3, 1, ZeldaScripts.CreateDoor(0, lookup));
            builder.AddScript(0, ZeldaScripts.CreateKey(0, lookup));
            builder.AddScript(0, ZeldaScripts.CreateKey(0, lookup));
            builder.AddScript(0, ZeldaScripts.CreateKey(0, lookup));
            var crawler = builder.Build();

            var initialState = emptyVariables.ToState(builder.Lookup);
            crawler.Crawl(new[] { initialState }, maxSteps);

            // test for exit reached
            Assert.True(crawler.DebugGetStep(0, initialState).HasDistanceFromExit);
        }

        [Test]
        public void CrawlGenome()
        {
            // 3--2, 0--7, Switch: 4, Key: 3, Door: 9--8, Key: 9, Door: 4--6, 9--0, Key: 8, Door: 8--5, Piston: 7--1, Key: 7, Door: 5--6, Piston: 2--6
            // 0--7, 0--9, 2--3, Key: 3, Key: 7, Key: 8, Key: 9, Door: 4--6, Door: 5--6, Door: 5--8, Door: 8--9, Switch: 4, Piston: 1--7, Piston: 2--6

            var builder = new ZeldaDungeonBuilder();
            var lookup = builder.Lookup;

            // 0--7, 0--9, 2--3
            builder.AddUndirectedTransition(0, 7, IdentityScript.Default);
            builder.AddUndirectedTransition(0, 9, IdentityScript.Default);
            builder.AddUndirectedTransition(2, 3, IdentityScript.Default);

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

            // Switch: 4, Piston: 1--7, Piston: 2--6
            builder.AddScript(4, ZeldaScripts.CreateColorSwitch(0, lookup));

            // Piston: 1--7, Piston: 2--6
            builder.AddUndirectedTransition(1, 7, ZeldaScripts.CreateColorPiston(0, ColorPistonScript.BluePistonState, lookup));
            builder.AddUndirectedTransition(2, 6, ZeldaScripts.CreateColorPiston(0, ColorPistonScript.RedPistonState, lookup));

            var crawler = builder.Build();
            var dot = new DotBuilder();
            crawler.Express(dot);

            var initialState = emptyVariables.ToState(builder.Lookup);
            crawler.Crawl(new[] { initialState }, 10000);

            // test for exit reached
            Assert.True(crawler.DebugGetStep(0, initialState).HasDistanceFromExit);
        }
    }
}
