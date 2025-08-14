using Lumpn.Profiling;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lumpn.Dungeon2
{
    using Locations = IDictionary<int, Location>;

    public sealed class Crawler
    {
        private const int entranceId = 1;
        private const int exitId = 0;

        private readonly Locations locations;
        private readonly Memory memory, buffer;

        private readonly HashSet<State> states = new HashSet<State>(StateEqualityComparer.Default);

        private readonly List<Step> steps = new List<Step>();
        private readonly HashSet<Step> uniqueSteps = new HashSet<Step>(StepEqualityComparer.Default);
        private readonly List<KeyValuePair<int, int>> traceEdges = new List<KeyValuePair<int, int>>();

        public Crawler(Locations locations, int stateSize)
        {
            this.locations = locations;
            this.memory = new Memory(stateSize, 1000);
            this.buffer = new Memory(stateSize, 1);
        }

        public void Express(DotBuilder builder)
        {
            builder.Begin();
            foreach (var location in locations.Values)
            {
                location.Express(builder);
            }
            builder.End();
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

        public List<Step> Crawl(int maxSteps)
        {
            // initialize initial steps
            var initialStateBuilder = new StateBuilder(buffer);
            var initialState = GetOrCreateState(initialStateBuilder);
            var initialSteps = new List<Step>();
            if (AddStep(entranceId, initialState, 0, out var initialStep))
            {
                initialSteps.Add(initialStep);
            }

            // forward pass
            Profiler.BeginSample("CrawlForward");
            var terminalSteps = CrawlForward(initialSteps, maxSteps);
            Profiler.EndSample();

            // initialize distance from exit
            for (int i = 0; i < terminalSteps.Count; i++)
            {
                var terminalStep = terminalSteps[i];
                terminalStep.SetDistanceFromExit(0);
                terminalSteps[i] = terminalStep;
                steps[terminalStep.id] = terminalStep;
            }

            // backward pass
            Profiler.BeginSample("CrawBackward");
            CrawlBackward(terminalSteps);
            Profiler.EndSample();

            return terminalSteps;
        }

        public List<Step> GetDeadEnds()
        {
            return steps.Where(p => !p.HasDistanceFromExit).ToList();
        }

        private List<Step> CrawlForward(List<Step> initialSteps, int maxSteps)
        {
            // keep track of terminals
            var terminalSteps = new List<Step>();

            // initialize BFS
            var queue = new Queue<Step>(initialSteps);
            int visitedSteps = 0;

            // crawl!
            var stateBuilder = new StateBuilder(buffer);
            var recorder = new Recorder("Execute");
            while (queue.Count > 0 && (visitedSteps < maxSteps))
            {
                // fetch step
                Step step = queue.Dequeue();
                visitedSteps++;

                // track terminals
                var locationId = step.locationId;
                if (locationId == exitId)
                {
                    terminalSteps.Add(step);
                }

                // try every transition
                var location = locations[locationId];
                var state = step.state;
                int nextDistanceFromEntrance = step.distanceFromEntrance + 1;
                foreach (var transition in location.Transitions)
                {
                    // execute transition
                    var nextLocation = transition.destinationId;
                    recorder.Begin();
                    var result = transition.Execute(state, stateBuilder);
                    recorder.End();

                    if (result == ScriptResult.Fail) continue; // transition impassable

                    // deduplicate state
                    var nextState = (result == ScriptResult.Pass) ? state : GetOrCreateState(stateBuilder);

                    // deduplicate step
                    if (AddStep(nextLocation, nextState, nextDistanceFromEntrance, out var nextStep))
                    {
                        // location reached with new state -> enqueue
                        queue.Enqueue(nextStep);
                    }

                    // connect steps
                    Connect(step, nextStep);
                }
            }
            recorder.Submit();

            return terminalSteps;
        }

        private void CrawlBackward(List<Step> terminalSteps)
        {
            // compute predecessor lookup
            Profiler.BeginSample("Lookup");
            var predecessors = traceEdges.ToLookup(p => p.Value, p => p.Key);
            Profiler.EndSample();

            // initialize BFS
            var queue = new Queue<Step>(terminalSteps);

            // crawl
            while (queue.Count > 0)
            {
                // fetch step
                var step = queue.Dequeue();

                // try every predecessor
                int prevDistanceFromExit = step.distanceFromExit + 1;
                var prevSteps = predecessors[step.id];
                foreach (var prevStepId in prevSteps)
                {
                    var prevStep = steps[prevStepId];
                    if (prevStep.HasDistanceFromExit) continue;

                    // unseen step reached -> enqueue
                    prevStep.SetDistanceFromExit(prevDistanceFromExit);
                    steps[prevStep.id] = prevStep;
                    queue.Enqueue(prevStep);
                }
            }
        }

        private State GetOrCreateState(StateBuilder builder)
        {
            // deduplicate
            var tmpState = builder.GetState();
            if (states.TryGetValue(tmpState, out var existingState))
            {
                return existingState;
            }

            // allocate
            var state = memory.Allocate();
            tmpState.CopyTo(state);

            // track
            states.Add(state);
            return state;
        }

        private bool AddStep(int locationId, State state, int distanceFromEntrance, out Step step)
        {
            var tmpStep = new Step(steps.Count, locationId, state, distanceFromEntrance);
            if (uniqueSteps.TryGetValue(tmpStep, out var existingStep))
            {
                step = existingStep;
                return false;
            }
            uniqueSteps.Add(tmpStep);
            steps.Add(tmpStep);
            step = tmpStep;
            return true;
        }

        private void Connect(Step step, Step nextStep)
        {
            traceEdges.Add(KeyValuePair.Create(step.id, nextStep.id));
        }
    }
}
