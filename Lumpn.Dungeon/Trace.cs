using Lumpn.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lumpn.Dungeon
{
    public sealed class Trace
    {
        private HashSet<Location> locations = new HashSet<Location>();

        public int CountSteps()
        {
            return locations.SelectMany(p => p.DebugGetSteps())
                            .Count();
        }

        public int CountSteps(int locationId)
        {
            return locations.Where(p => p.Id == locationId)
                            .SelectMany(p => p.DebugGetSteps())
                            .Count();
        }

        public bool ContainsStep(int locationId, State state)
        {
            return locations.Where(p => p.Id == locationId)
                            .Select(p => p.DebugGetStep(state))
                            .Any(p => p != null);
        }


        public int CountSuccessors(int locationId, State state)
        {
            return locations.Where(p => p.Id == locationId)
                            .Select(p => p.DebugGetStep(state))
                            .Where(p => p != null)
                            .SelectMany(p => p.Successors)
                            .Count();
        }

        public int CountDeadEnds()
        {
            return locations.SelectMany(p => p.DebugGetSteps())
                            .Count(p => !p.HasDistanceFromExit);
        }

        public bool HasDistanceFromExit(int locationId, State state)
        {
            return locations.Where(p => p.Id == locationId)
                            .Select(p => p.DebugGetStep(state))
                            .Any(p => p != null && p.HasDistanceFromExit);
        }

        public double CalcRevisitFactor()
        {
            int numSteps = locations.SelectMany(p => p.DebugGetSteps()).Count();
            int numLocations = locations.Count();
            return (double)numSteps / (numLocations + 1);
        }

        public double CalcBranchFactor()
        {
            int numSteps = 0;
            int numBranches = 0;
            foreach (var step in locations.SelectMany(p => p.DebugGetSteps()))
            {
                numBranches += step.Successors.Count();
                numSteps++;
            }

            return (double)numBranches / (numSteps + 1);
        }

        public int CalcShortestPathLength(int locationId)
        {
            return locations.Where(p => p.Id == locationId)
                            .SelectMany(p => p.DebugGetSteps())
                            .Select(p => p.DistanceFromEntrance)
                            .MinOrFallback(-1);
        }

        public bool AddStep(Location location, State state, int distanceFromEntrance, out Step step)
        {
            locations.Add(location);
            return location.AddStep(state, distanceFromEntrance, out step);
        }

        public void Connect(Step prevStep, Step nextStep)
        {
            nextStep.AddPredecessor(prevStep);
            prevStep.AddSuccessor(nextStep);
        }

        public void Finalize(List<Step> terminalSteps)
        {
            // initialize distance from exit
            foreach (var step in terminalSteps)
            {
                step.SetDistanceFromExit(0);
            }

            CrawlBackward(terminalSteps);
        }

        private static void CrawlBackward(List<Step> terminalSteps)
        {
            // initialize BFS
            var queue = new Queue<Step>(terminalSteps);

            // crawl
            while (queue.Count > 0)
            {
                // fetch step
                var step = queue.Dequeue();

                // try every predecessor
                int prevDistanceFromExit = step.DistanceFromExit + 1;
                foreach (var prevStep in step.Predecessors)
                {
                    if (prevStep.HasDistanceFromExit) continue;

                    // unseen step reached -> enqueue
                    prevStep.SetDistanceFromExit(prevDistanceFromExit);
                    queue.Enqueue(prevStep);
                }
            }
        }

        public void PrintStates(VariableLookup lookup)
        {
            PrintStates(lookup, Console.Out);
        }

        public void PrintStates(VariableLookup lookup, int dungeonId)
        {
            PrintStates(lookup, $"dungeon{dungeonId}-states.dot");
        }

        public void PrintStates(VariableLookup lookup, string fileName)
        {
            using (var writer = File.CreateText(fileName))
            {
                PrintStates(lookup, writer);
            }
        }

        public void PrintStates(VariableLookup lookup, TextWriter writer)
        {
            var steps = locations.SelectMany(p => p.DebugGetSteps()).ToArray();
            var states = steps.Select(p => p.State).Distinct(StateEqualityComparer.Default).ToArray();
            var allVariables = Enumerable.Range(0, lookup.NumVariables).Select(lookup.QueryNamed).Where(p => p != null).ToArray();

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

        public void PrintSteps(VariableLookup lookup)
        {
            PrintSteps(lookup, Console.Out);
        }

        public void PrintSteps(VariableLookup lookup, int dungeonId)
        {
            PrintSteps(lookup, $"dungeon{dungeonId}-trace.dot");
        }

        public void PrintSteps(VariableLookup lookup, string fileName)
        {
            using (var writer = File.CreateText(fileName))
            {
                PrintSteps(lookup, writer);
            }
        }

        public void PrintSteps(VariableLookup lookup, TextWriter writer)
        {
            var steps = locations.SelectMany(p => p.DebugGetSteps()).ToArray();
            var allVariables = Enumerable.Range(0, lookup.NumVariables).Select(lookup.QueryNamed).Where(p => p != null).ToArray();

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
