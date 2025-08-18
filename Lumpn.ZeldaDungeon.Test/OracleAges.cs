using Lumpn.Dungeon;
using Lumpn.Dungeon.Scripts;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace Lumpn.ZeldaDungeon.Test
{
    [TestFixture]
    public sealed class OracleAges
    {
        private const int maxSteps = 10000;

        private const int smallKey = 0;
        private const int teleporter = 7;

        [Test]
        public void JabuJabuBelly()
        {
            // https://www.zeldadungeon.net/oracle-of-ages-walkthrough/jabu-jabus-belly/#c7_5

            var lookup = new VariableLookup();
            var noOp = NoOpScript.instance;
            var setWater1 = new SetScript(1, "waterLevel", lookup);
            var setWater2 = new SetScript(2, "waterLevel", lookup);
            var setWater3 = new SetScript(3, "waterLevel", lookup);
            var water1 = new EqualsScript(1, "water (1)", "waterLevel", lookup);
            var water2 = new EqualsScript(2, "water (2)", "waterLevel", lookup);
            var water3 = new EqualsScript(3, "water (3)", "waterLevel", lookup);
            var water12 = new LessThanScript(3, "water (1/2)", "waterLevel", lookup);
            var water23 = new GreaterThanScript(1, "water (2/3)", "waterLevel", lookup);
            var getLongHook = new AcquireScript("longHook", lookup);
            var longHook = new GreaterThanScript(0, "longGap", "longHook", lookup);
            var getBossKey = new AcquireScript("bossKey", lookup);
            var bossDoor = new GreaterThanScript(0, "bossDoor", "bossKey", lookup);
            var longHookAndWater1 = new AndScript(longHook, water1);
            var longHookAndWater3 = new AndScript(longHook, water3);
            var getTeleporter = new AcquireScript("teleporter", lookup);
            var teleporter = new EqualsScript(1, "teleporter", "teleporter", lookup);

            var builder = new CrawlerBuilder();
            builder.AddUndirectedTransition(1, 11, noOp);
            builder.AddScript(11, CreateKey(lookup));
            builder.AddDirectedTransition(11, 12, water1);
            builder.AddDirectedTransition(11, 12, longHook);
            builder.AddDirectedTransition(12, 11, noOp);
            builder.AddDirectedTransition(12, 13, noOp);
            builder.AddDirectedTransition(13, 11, noOp);
            builder.AddUndirectedTransition(13, 92, water23);
            builder.AddScript(92, getTeleporter);
            builder.AddDirectedTransition(92, 14, teleporter);
            builder.AddDirectedTransition(14, 92, noOp);
            builder.AddUndirectedTransition(14, 15, CreateDoor(lookup));
            builder.AddScript(15, getLongHook);
            builder.AddDirectedTransition(14, 12, noOp);
            builder.AddUndirectedTransition(11, 91, water23);
            builder.AddDirectedTransition(91, 94, longHook);
            builder.AddDirectedTransition(94, 91, noOp);
            builder.AddUndirectedTransition(94, 16, noOp);
            builder.AddScript(16, CreateKey(lookup));
            builder.AddDirectedTransition(16, 17, noOp);
            builder.AddDirectedTransition(17, 11, water1);
            builder.AddUndirectedTransition(11, 18, water1);
            builder.AddScript(18, CreateKey(lookup));
            builder.AddUndirectedTransition(11, 19, longHookAndWater1);
            builder.AddScript(19, CreateKey(lookup));
            builder.AddUndirectedTransition(11, 13, teleporter);

            builder.AddUndirectedTransition(21, 22, water23);
            builder.AddUndirectedTransition(21, 22, longHook);
            builder.AddUndirectedTransition(21, 201, water23);
            builder.AddUndirectedTransition(22, 201, water23);
            builder.AddUndirectedTransition(201, 202, water3);
            builder.AddScript(202, CreateKey(lookup));
            builder.AddUndirectedTransition(22, 23, water12);
            builder.AddDirectedTransition(23, 22, noOp);
            builder.AddUndirectedTransition(23, 24, CreateDoor(lookup));
            builder.AddUndirectedTransition(21, 26, longHook);
            builder.AddDirectedTransition(26, 21, noOp);
            builder.AddUndirectedTransition(25, 27, water23);
            builder.AddUndirectedTransition(25, 26, water23);
            builder.AddUndirectedTransition(26, 27, water23);
            builder.AddScript(27, CreateKey(lookup));
            builder.AddUndirectedTransition(28, 0, bossDoor);
            builder.AddDirectedTransition(28, 25, noOp);
            builder.AddDirectedTransition(28, 26, noOp);

            builder.AddUndirectedTransition(17, 21, water23);
            builder.AddUndirectedTransition(17, 22, water23);
            builder.AddUndirectedTransition(17, 201, water23);
            builder.AddUndirectedTransition(11, 21, water2);
            builder.AddDirectedTransition(22, 11, noOp);
            builder.AddUndirectedTransition(13, 28, water3);
            builder.AddUndirectedTransition(13, 25, water23);
            builder.AddUndirectedTransition(12, 25, noOp);

            builder.AddDirectedTransition(21, 11, water1);
            builder.AddDirectedTransition(21, 17, water1);
            builder.AddDirectedTransition(22, 17, water1);
            builder.AddDirectedTransition(201, 17, water1);
            builder.AddDirectedTransition(25, 13, water1);
            builder.AddDirectedTransition(25, 12, water1);
            builder.AddDirectedTransition(26, 12, water1);
            builder.AddDirectedTransition(27, 12, water1);

            builder.AddUndirectedTransition(32, 33, CreateDoor(lookup));
            builder.AddDirectedTransition(32, 34, longHook);
            builder.AddDirectedTransition(34, 32, noOp);
            builder.AddUndirectedTransition(34, 35, CreateDoor(lookup));
            builder.AddUndirectedTransition(35, 36, CreateDoor(lookup));
            builder.AddUndirectedTransition(36, 37, CreateDoor(lookup));
            builder.AddUndirectedTransition(37, 38, CreateDoor(lookup));
            builder.AddScript(38, getBossKey);
            builder.AddDirectedTransition(38, 34, noOp);
            builder.AddUndirectedTransition(32, 39, longHookAndWater3);
            builder.AddScript(39, CreateKey(lookup));
            builder.AddDirectedTransition(35, 31, noOp);
            builder.AddScript(31, setWater1);
            builder.AddScript(33, setWater2);
            builder.AddScript(35, setWater3);

            builder.AddUndirectedTransition(24, 31, noOp);
            builder.AddUndirectedTransition(25, 32, noOp);
            //PrintMission(builder, lookup, 6);

            builder.MergeNodes(noOp);
            //PrintMission(builder, lookup, 61);

            var initialVariables = new VariableAssignment();
            initialVariables.Assign("waterLevel", 2);

            var crawler = builder.Build();
            var initialState = initialVariables.ToState(lookup);
            var terminalSteps = crawler.Crawl(new[] { initialState }, maxSteps);

            //PrintStates(crawler, lookup, 61);
            //PrintTrace(crawler, lookup, 61);

            Assert.IsNotEmpty(terminalSteps);
            Assert.AreEqual(1, terminalSteps.Count);
        }

        private static Script CreateKey(VariableLookup lookup)
        {
            return new AcquireScript("smallKey", lookup);
        }

        private static Script CreateDoor(VariableLookup lookup)
        {
            return new ConsumeScript("door", "smallKey", lookup);
        }

        private static void PrintMission(CrawlerBuilder crawler, VariableLookup lookup, int dungeonId)
        {
            using (var writer = File.CreateText($"dungeon{dungeonId}-mission.dot"))
            {
                var dot = new DotBuilder(writer);
                crawler.Express(dot, NoOpScript.instance);
            }
        }

        private static void PrintStates(Crawler crawler, VariableLookup lookup, int dungeonId)
        {
            var steps = crawler.DebugGetSteps().ToArray();
            var states = steps.Select(p => p.State).Distinct(StateEqualityComparer.Default).ToArray();
            var allVariables = Enumerable.Range(0, lookup.NumVariables).Select(lookup.QueryNamed).Where(p => p != null).ToArray();

            using (var writer = File.CreateText($"dungeon{dungeonId}-states.dot"))
            {
                var dot = new DotBuilder(writer);
                dot.Begin();
                for (int i = 0; i < states.Length; i++)
                {
                    var state = states[i];
                    var vars = allVariables.Select(p => KeyValuePair.Create(p, state.Get(p, 0))).Where(p => p.Value != 0);
                    dot.AddNode(i, string.Join("\\n", vars.Select(p => $"{p.Key}: {p.Value}")));
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
        }

        private static void PrintTrace(Crawler crawler, VariableLookup lookup, int dungeonId)
        {
            var steps = crawler.DebugGetSteps().ToArray();
            var allVariables = Enumerable.Range(0, lookup.NumVariables).Select(lookup.QueryNamed).Where(p => p != null).ToArray();

            using (var writer = File.CreateText($"dungeon{dungeonId}-trace.dot"))
            {
                var dot = new DotBuilder(writer);
                dot.Begin();
                for (int i = 0; i < steps.Length; i++)
                {
                    var step = steps[i];

                    var state = step.State;
                    var vars = allVariables.Select(p => KeyValuePair.Create(p, state.Get(p, 0))).Where(p => p.Value != 0);

                    var shape = (step.DistanceFromExit == 0) ? "\", shape=\"box" : string.Empty;
                    dot.AddNode(i, $"{step.Location}\\n{string.Join("\\n", vars.Select(p => $"{p.Key}: {p.Value}"))}{shape}");

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
        }
    }
}
