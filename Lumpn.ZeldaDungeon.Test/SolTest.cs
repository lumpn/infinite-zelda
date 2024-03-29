﻿using System.Linq;
using Lumpn.Dungeon;
using NUnit.Framework;

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

        private const int maxSteps = 10000;

        private static readonly VariableAssignment emptyVariables = new VariableAssignment();

        [Test]
        public void CrawlChapter1()
        {
            var builder = new ZeldaDungeonBuilder();
            var lookup = builder.Lookup;

            builder.AddUndirectedTransition(0, 1, IdentityScript.Default);
            builder.AddUndirectedTransition(1, 2, IdentityScript.Default);
            builder.AddUndirectedTransition(2, 3, IdentityScript.Default);
            builder.AddUndirectedTransition(2, 4, IdentityScript.Default);
            builder.AddUndirectedTransition(2, 5, CreateDoor(solarPanel, lookup));
            builder.AddUndirectedTransition(5, 6, IdentityScript.Default);
            builder.AddUndirectedTransition(6, 7, IdentityScript.Default);
            builder.AddUndirectedTransition(7, 8, IdentityScript.Default);
            builder.AddUndirectedTransition(7, 9, IdentityScript.Default);
            builder.AddUndirectedTransition(9, 10, IdentityScript.Default);
            builder.AddUndirectedTransition(9, 11, IdentityScript.Default);
            builder.AddUndirectedTransition(11, 12, IdentityScript.Default);
            builder.AddUndirectedTransition(11, 13, IdentityScript.Default);
            builder.AddUndirectedTransition(11, 14, CreateDoor(generator, lookup));
            builder.AddUndirectedTransition(14, 15, CreateDoor(solarPanel, lookup));
            builder.AddUndirectedTransition(15, 16, IdentityScript.Default);
            builder.AddUndirectedTransition(16, 17, IdentityScript.Default);
            builder.AddUndirectedTransition(15, 6, CreateObstacle(routeInfo, lookup));
            builder.AddUndirectedTransition(6, 18, CreateObstacle(clearance, lookup));
            builder.AddUndirectedTransition(4, 19, CreateObstacle(clearance, lookup));
            builder.AddUndirectedTransition(19, 20, IdentityScript.Default);
            builder.AddUndirectedTransition(20, 21, CreateObstacle(laser, lookup));
            builder.AddUndirectedTransition(21, 22, IdentityScript.Default);

            builder.AddScript(4, CreateItem(solarPanel, lookup));
            builder.AddScript(12, CreateItem(generator, lookup));
            builder.AddScript(13, CreateItem(solarPanel, lookup));
            builder.AddScript(15, CreateItem(routeInfo, lookup));
            builder.AddScript(17, CreateItem(clearance, lookup));
            builder.AddScript(18, CreateItem(laser, lookup));

            var crawler = builder.Build();

            var initialState = emptyVariables.ToState(lookup);
            crawler.Crawl(new[] { initialState }, maxSteps);

            PrintStates(crawler, lookup);
        }

        [Test]
        public void CrawlChapter1Reduced()
        {
            var builder = new ZeldaDungeonBuilder();
            var lookup = builder.Lookup;

            // 0: 1-4
            // 1: 5-13
            // 2: 14
            // 3: 15-17
            // 4: 18
            // 5: 19-20
            // 6: 21-22

            builder.AddUndirectedTransition(0, 1, CreateDoor(solarPanel, lookup));
            builder.AddUndirectedTransition(1, 2, CreateDoor(generator, lookup));
            builder.AddUndirectedTransition(2, 3, CreateDoor(solarPanel, lookup));
            builder.AddUndirectedTransition(3, 1, CreateObstacle(routeInfo, lookup));
            builder.AddUndirectedTransition(1, 4, CreateObstacle(clearance, lookup));
            builder.AddUndirectedTransition(0, 5, CreateObstacle(clearance, lookup));
            builder.AddUndirectedTransition(5, 6, CreateObstacle(laser, lookup));

            builder.AddScript(0, CreateItem(solarPanel, lookup));
            builder.AddScript(1, CreateItem(generator, lookup));
            builder.AddScript(1, CreateItem(solarPanel, lookup));
            builder.AddScript(3, CreateItem(routeInfo, lookup));
            builder.AddScript(3, CreateItem(clearance, lookup));
            builder.AddScript(4, CreateItem(laser, lookup));

            var crawler = builder.Build();

            var initialState = emptyVariables.ToState(lookup);
            crawler.Crawl(new[] { initialState }, maxSteps);

            PrintMission(crawler, lookup);
            PrintStates(crawler, lookup);
            PrintSteps(crawler, lookup);
        }

        [Test]
        public void CrawlChapter1Plus()
        {
            var builder = new ZeldaDungeonBuilder();
            var lookup = builder.Lookup;

            builder.AddUndirectedTransition(0, 1, IdentityScript.Default);
            builder.AddUndirectedTransition(1, 2, IdentityScript.Default);
            builder.AddUndirectedTransition(2, 3, IdentityScript.Default);
            builder.AddUndirectedTransition(2, 4, IdentityScript.Default);
            builder.AddUndirectedTransition(2, 5, CreateDoor(solarPanel, lookup));
            builder.AddUndirectedTransition(5, 6, IdentityScript.Default);
            builder.AddUndirectedTransition(6, 7, IdentityScript.Default);
            builder.AddUndirectedTransition(7, 8, IdentityScript.Default);
            builder.AddUndirectedTransition(7, 9, IdentityScript.Default);
            builder.AddUndirectedTransition(9, 10, IdentityScript.Default);
            builder.AddUndirectedTransition(9, 11, IdentityScript.Default);
            builder.AddUndirectedTransition(11, 12, IdentityScript.Default);
            builder.AddUndirectedTransition(11, 13, IdentityScript.Default);
            builder.AddUndirectedTransition(11, 14, CreateDoor(generator, lookup));
            builder.AddUndirectedTransition(14, 15, CreateDoor(solarPanel, lookup));
            builder.AddUndirectedTransition(15, 16, IdentityScript.Default);
            builder.AddUndirectedTransition(16, 17, IdentityScript.Default);
            builder.AddUndirectedTransition(15, 6, CreateObstacle(routeInfo, lookup));
            builder.AddUndirectedTransition(6, 18, CreateObstacle(clearance, lookup));
            builder.AddUndirectedTransition(4, 19, CreateObstacle(clearance, lookup));
            builder.AddUndirectedTransition(19, 20, IdentityScript.Default);
            builder.AddUndirectedTransition(20, 21, CreateObstacle(laser, lookup));
            builder.AddUndirectedTransition(21, 22, IdentityScript.Default);

            builder.AddScript(1, CreateItem("item1", lookup));

            builder.AddScript(4, CreateTrade("item1", solarPanel, lookup));
            builder.AddScript(12, CreateItem(generator, lookup));
            builder.AddScript(13, CreateItem(solarPanel, lookup));
            builder.AddScript(15, CreateItem(routeInfo, lookup));
            builder.AddScript(17, CreateItem(clearance, lookup));
            builder.AddScript(18, CreateItem(laser, lookup));

            // deliver
            //builder.AddScript(1, CreateItem("item1", lookup));
            //builder.AddScript(3, CreateDoor("item1", lookup));

            // trade
            //builder.AddScript(1, CreateItem("item2", lookup));
            //builder.AddScript(6, CreateTrade("item2", "item3", lookup));
            //builder.AddScript(21, CreateDoor("item3", lookup));

            var crawler = builder.Build();

            var initialState = emptyVariables.ToState(lookup);
            crawler.Crawl(new[] { initialState }, maxSteps);

            PrintStates(crawler, lookup);
        }

        private static void PrintMission(Crawler crawler, VariableLookup lookup)
        {
            var dot = new DotBuilder();
            crawler.Express(dot);
        }

        private static void PrintStates(Crawler crawler, VariableLookup lookup)
        {
            var steps = crawler.DebugGetSteps().ToArray();
            var states = steps.Select(p => p.State).Distinct(StateEqualityComparer.Default).ToArray();
            var allVariables = Enumerable.Range(0, lookup.NumVariables).Select(lookup.Query).Where(p => p != null).ToArray();

            var dot = new DotBuilder();
            dot.Begin();
            for (int i = 0; i < states.Length; i++)
            {
                var state = states[i];
                var vars = allVariables.Where(p => state.Get(p, 0) > 0);
                dot.AddNode(i, string.Join(", ", vars));
            }

            foreach (var step in steps)
            {
                var state = step.State;
                foreach (var succ in step.Successors)
                {
                    var succState = succ.State;
                    if (state.Equals(succState)) continue;

                    var id1 = System.Array.IndexOf(states, state);
                    var id2 = System.Array.IndexOf(states, succState);

                    dot.AddEdge(id1, id2, $"{step.Location} &rarr; {succ.Location}");
                }
            }
            dot.End();
        }

        private static void PrintSteps(Crawler crawler, VariableLookup lookup)
        {
            var steps = crawler.DebugGetSteps().ToArray();
            var allVariables = Enumerable.Range(0, lookup.NumVariables).Select(lookup.Query).Where(p => p != null).ToArray();

            var dot = new DotBuilder();
            dot.Begin();
            for (int i = 0; i < steps.Length; i++)
            {
                var step = steps[i];

                var state = step.State;
                var vars = allVariables.Where(p => state.Get(p, 0) > 0);

                dot.AddNode(i, $"Location {step.Location}\n{string.Join(", ", vars)}");

                foreach (var succ in step.Successors)
                {
                    var succIndex = System.Array.IndexOf(steps, succ);
                    var succState = succ.State;
                    var label = string.Empty;
                    if (!state.Equals(succState))
                    {
                        label = "\", style=bold, color=\"maroon";
                    }
                    dot.AddEdge(i, succIndex, label);
                }
            }
            dot.End();
        }

        private static ItemScript CreateItem(string item, VariableLookup lookup)
        {
            return new ItemScript(lookup.Resolve(item), lookup);
        }

        private static TradeScript CreateTrade(string item1, string item2, VariableLookup lookup)
        {
            return new TradeScript(lookup.Resolve(item1), lookup.Resolve(item2), lookup, $"{item1} &rarr; {item2}");
        }

        private static DoorScript CreateDoor(string item, VariableLookup lookup)
        {
            return new DoorScript(lookup.Resolve(item), lookup, $"Use {item}");
        }

        private static ObstacleScript CreateObstacle(string item, VariableLookup lookup)
        {
            return new ObstacleScript(lookup.Resolve(item), $"Has {item}");
        }
    }
}
