using Lumpn.Profiling;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lumpn.Dungeon2
{
    using TraceEdge = KeyValuePair<int, int>;

    public sealed class Trace
    {
        private const int initialCapacitySteps = 10000;

        private readonly List<Step> steps = new List<Step>(initialCapacitySteps);
        private readonly HashSet<Step> uniqueSteps = new HashSet<Step>(initialCapacitySteps, StepEqualityComparer.Default);
        private readonly List<TraceEdge> traceEdges = new List<TraceEdge>(initialCapacitySteps);

        public int CountSteps(int locationId)
        {
            return steps.Count(p => p.locationId == locationId);
        }

        public bool AddStep(int locationId, State state, int distanceFromEntrance, out Step step)
        {
            var tmpStep = new Step(steps.Count, locationId, state, distanceFromEntrance);
            if (uniqueSteps.Contains(tmpStep))
            {
                if (uniqueSteps.TryGetValue(tmpStep, out var existingStep))
                {
                    step = existingStep;
                    return false;
                }
            }
            uniqueSteps.Add(tmpStep);
            steps.Add(tmpStep);
            step = tmpStep;
            return true;
        }

        public void Connect(Step step, Step nextStep)
        {
            traceEdges.Add(KeyValuePair.Create(step.id, nextStep.id));
        }

        public void Finalize(List<Step> terminalSteps)
        {
            // initialize distance from exit
            for (int i = 0; i < terminalSteps.Count; i++)
            {
                var terminalStep = terminalSteps[i];
                terminalStep.SetDistanceFromExit(0);
                terminalSteps[i] = terminalStep;
                steps[terminalStep.id] = terminalStep;
            }

            // backward pass
            CrawlBackward(terminalSteps);
        }

        private void CrawlBackward(List<Step> terminalSteps)
        {
            // compute predecessor lookup
            Profiler.BeginSample("Lookup");
            traceEdges.Sort(ByDestination);
            var groups = new int[steps.Count + 1];
            var currentGroup = -1;
            for (int i = 0; i < traceEdges.Count; i++)
            {
                var group = traceEdges[i].Value;
                if (group != currentGroup)
                {
                    groups[group] = i;
                    currentGroup = group;
                }
            }
            groups[currentGroup + 1] = traceEdges.Count;
            Profiler.EndSample();

            // initialize BFS
            var queue = new Queue<Step>(1000);
            foreach (var step in terminalSteps)
            {
                queue.Enqueue(step);
            }

            // crawl
            while (queue.Count > 0)
            {
                // fetch step
                var step = queue.Dequeue();

                // try every predecessor
                int prevDistanceFromExit = step.distanceFromExit + 1;
                var start = groups[step.id];
                var end = groups[step.id + 1];
                for (var i = start; i < end; i++)
                {
                    var prevStepId = traceEdges[i].Key;
                    var prevStep = steps[prevStepId];
                    if (prevStep.HasDistanceFromExit) continue;

                    // unseen step reached -> enqueue
                    prevStep.SetDistanceFromExit(prevDistanceFromExit);
                    steps[prevStep.id] = prevStep;
                    queue.Enqueue(prevStep);
                }
            }
        }

        private static int ByDestination(TraceEdge a, TraceEdge b)
        {
            return (a.Value - b.Value);
        }

        public void PrintTrace(DotBuilder builder)
        {
            builder.Begin();
            foreach (var step in steps)
            {
                step.Print(builder);
            }
            foreach (var edge in traceEdges)
            {
                builder.AddEdge(edge.Key, edge.Value, string.Empty);
            }
            builder.End();
        }

        public List<Step> GetDeadEnds()
        {
            return steps.Where(p => !p.HasDistanceFromExit).ToList();
        }
    }
}
