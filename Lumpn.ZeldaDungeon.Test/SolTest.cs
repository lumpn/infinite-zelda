using Lumpn.Dungeon;
using Lumpn.Dungeon.Scripts;
using NUnit.Framework;
using System.Linq;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace Lumpn.ZeldaDungeon.Test
{
    [TestFixture]
    public sealed class SolTest
    {
        private const string solarPanel = "Solar Panel";
        private const string generator = "Generator";
        private const string routeInfo = "Route Information";
        private const string clearance = "Security Clearance";
        private const string laser = "Ignition Laser";
        private const string drone = "Drone";
        private const string repairKit = "Repair Kit";

        private const int maxSteps = 10000;

        private static readonly VariableAssignment emptyVariables = new VariableAssignment();

        [Test]
        public void CrawlChapter1()
        {
            var builder = new ZeldaDungeonBuilder();
            var lookup = builder.Lookup;

            builder.AddUndirectedTransition(1, 2, NoOpScript.instance);
            builder.AddUndirectedTransition(2, 3, NoOpScript.instance);
            builder.AddUndirectedTransition(2, 4, NoOpScript.instance);
            builder.AddUndirectedTransition(2, 5, CreateDoor(solarPanel, lookup));
            builder.AddUndirectedTransition(5, 6, NoOpScript.instance);
            builder.AddUndirectedTransition(6, 7, NoOpScript.instance);
            builder.AddUndirectedTransition(7, 8, NoOpScript.instance);
            builder.AddUndirectedTransition(7, 9, NoOpScript.instance);
            builder.AddUndirectedTransition(9, 10, NoOpScript.instance);
            builder.AddUndirectedTransition(9, 11, NoOpScript.instance);
            builder.AddUndirectedTransition(11, 12, NoOpScript.instance);
            builder.AddUndirectedTransition(11, 13, NoOpScript.instance);
            builder.AddUndirectedTransition(11, 14, CreateDoor(generator, lookup));
            builder.AddUndirectedTransition(14, 15, CreateDoor(solarPanel, lookup));
            builder.AddUndirectedTransition(15, 16, NoOpScript.instance);
            builder.AddUndirectedTransition(16, 17, NoOpScript.instance);
            builder.AddUndirectedTransition(15, 6, CreateObstacle(routeInfo, lookup));
            builder.AddUndirectedTransition(6, 18, CreateObstacle(clearance, lookup));
            builder.AddUndirectedTransition(4, 19, CreateObstacle(clearance, lookup));
            builder.AddUndirectedTransition(19, 20, NoOpScript.instance);
            builder.AddUndirectedTransition(20, 21, CreateObstacle(laser, lookup));
            builder.AddUndirectedTransition(21, 0, NoOpScript.instance);

            builder.AddScript(4, CreateItem(solarPanel, lookup));
            builder.AddScript(12, CreateItem(generator, lookup));
            builder.AddScript(13, CreateItem(solarPanel, lookup));
            builder.AddScript(15, CreateItem(routeInfo, lookup));
            builder.AddScript(17, CreateItem(clearance, lookup));
            builder.AddScript(18, CreateItem(laser, lookup));

            var crawler = builder.Build();
            var initialState = emptyVariables.ToState(lookup);
            var trace = crawler.Crawl(new[] { initialState }, maxSteps);

            PrintMission(crawler, lookup);
            trace.PrintStates(lookup);
            trace.PrintSteps(lookup);

            // test for exit reached
            Assert.True(trace.HasDistanceFromExit(1, initialState));
        }

        [Test]
        public void CrawlChapter1Reduced()
        {
            var builder = new ZeldaDungeonBuilder();
            var lookup = builder.Lookup;

            // 1: 1-4
            // 2: 5-13
            // 3: 14
            // 4: 15-17
            // 5: 18
            // 6: 19-20
            // 0: 21-0

            builder.AddUndirectedTransition(1, 2, CreateDoor(solarPanel, lookup));
            builder.AddUndirectedTransition(2, 3, CreateDoor(generator, lookup));
            builder.AddUndirectedTransition(3, 4, CreateDoor(solarPanel, lookup));
            builder.AddUndirectedTransition(4, 2, CreateObstacle(routeInfo, lookup));
            builder.AddUndirectedTransition(2, 5, CreateObstacle(clearance, lookup));
            builder.AddUndirectedTransition(1, 6, CreateObstacle(clearance, lookup));
            builder.AddUndirectedTransition(6, 0, CreateObstacle(laser, lookup));

            builder.AddScript(1, CreateItem(solarPanel, lookup));
            builder.AddScript(2, CreateItem(generator, lookup));
            builder.AddScript(2, CreateItem(solarPanel, lookup));
            builder.AddScript(4, CreateItem(routeInfo, lookup));
            builder.AddScript(4, CreateItem(clearance, lookup));
            builder.AddScript(5, CreateItem(laser, lookup));

            var crawler = builder.Build();
            var initialState = emptyVariables.ToState(lookup);
            var trace = crawler.Crawl(new[] { initialState }, maxSteps);

            PrintMission(crawler, lookup);
            trace.PrintStates(lookup);
            trace.PrintSteps(lookup);

            // test for exit reached
             Assert.True(trace.HasDistanceFromExit(1, initialState));
        }

        [Test]
        public void CrawlChapter1Plus()
        {
            var builder = new ZeldaDungeonBuilder();
            var lookup = builder.Lookup;

            builder.AddUndirectedTransition(1, 2, NoOpScript.instance);
            builder.AddUndirectedTransition(2, 3, NoOpScript.instance);
            builder.AddUndirectedTransition(2, 4, NoOpScript.instance);
            builder.AddUndirectedTransition(2, 5, CreateDoor(solarPanel, lookup));
            builder.AddUndirectedTransition(5, 6, NoOpScript.instance);
            builder.AddUndirectedTransition(6, 7, NoOpScript.instance);
            builder.AddUndirectedTransition(7, 8, NoOpScript.instance);
            builder.AddUndirectedTransition(7, 9, NoOpScript.instance);
            builder.AddUndirectedTransition(9, 10, NoOpScript.instance);
            builder.AddUndirectedTransition(9, 11, NoOpScript.instance);
            builder.AddUndirectedTransition(11, 12, NoOpScript.instance);
            builder.AddUndirectedTransition(11, 13, NoOpScript.instance);
            builder.AddUndirectedTransition(11, 14, CreateDoor(generator, lookup));
            builder.AddUndirectedTransition(14, 15, CreateDoor(solarPanel, lookup));
            builder.AddUndirectedTransition(15, 16, NoOpScript.instance);
            builder.AddUndirectedTransition(16, 17, NoOpScript.instance);
            builder.AddUndirectedTransition(15, 6, CreateObstacle(routeInfo, lookup));
            builder.AddUndirectedTransition(6, 18, CreateObstacle(clearance, lookup));
            builder.AddUndirectedTransition(4, 19, CreateObstacle(clearance, lookup));
            builder.AddUndirectedTransition(19, 20, NoOpScript.instance);
            builder.AddUndirectedTransition(20, 21, CreateObstacle(laser, lookup));
            builder.AddUndirectedTransition(21, 0, NoOpScript.instance);

            builder.AddScript(1, CreateItem(drone, lookup));
            builder.AddScript(3, CreateTrade(drone, repairKit, lookup));
            builder.AddScript(4, CreateTrade(repairKit, solarPanel, lookup));
            builder.AddScript(12, CreateItem(generator, lookup));
            builder.AddScript(13, CreateItem(solarPanel, lookup));
            builder.AddScript(15, CreateItem(routeInfo, lookup));
            builder.AddScript(17, CreateItem(clearance, lookup));
            builder.AddScript(18, CreateItem(laser, lookup));

            var crawler = builder.Build();
            var initialState = emptyVariables.ToState(lookup);
            var trace = crawler.Crawl(new[] { initialState }, maxSteps);

            PrintMission(crawler, lookup);
            trace.PrintStates(lookup);
            trace.PrintSteps( lookup);

            // test for exit reached
             Assert.True(trace.HasDistanceFromExit(1, initialState));
        }

        private static void PrintMission(Crawler crawler, VariableLookup lookup)
        {
            var dot = new DotBuilder();
            crawler.Express(dot);
        }


        private static AcquireScript CreateItem(string item, VariableLookup lookup)
        {
            return new AcquireScript(item, lookup);
        }

        private static TradeScript CreateTrade(string item1, string item2, VariableLookup lookup)
        {
            return new TradeScript($"{item1} &rarr; {item2}", item1, item2, lookup);
        }

        private static ConsumeScript CreateDoor(string item, VariableLookup lookup)
        {
            return new ConsumeScript($"Use {item}", item, lookup);
        }

        private static GreaterThanScript CreateObstacle(string item, VariableLookup lookup)
        {
            return new GreaterThanScript(0, $"Has {item}", item, lookup);
        }
    }
}
