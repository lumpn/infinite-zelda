using Lumpn.Profiling;
using System.Collections.Generic;

namespace Lumpn.Dungeon2
{
    using Locations = IDictionary<int, Location>;

    public sealed class Crawler
    {
        private const int initialCapacityStates = 1000;

        private const int entranceId = 1;
        private const int exitId = 0;

        private readonly Locations locations;
        private readonly Memory memory, buffer;

        private readonly HashSet<State> states = new HashSet<State>(initialCapacityStates, StateEqualityComparer.Default);

        public Crawler(Locations locations, int stateSize)
        {
            this.locations = locations;
            this.memory = new Memory(stateSize, initialCapacityStates);
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

        public Trace Crawl(int maxSteps)
        {
            var initialStateBuilder = new StateBuilder(buffer);
            var initialState = initialStateBuilder.GetState();
            return Crawl(initialState, maxSteps);
        }

        public Trace Crawl(State initialState, int maxSteps)
        {
            var trace = new Trace();

            // initialize initial steps
            var internalInitialState = GetOrCreateState(initialState);
            var initialSteps = new List<Step>();
            if (trace.AddStep(entranceId, internalInitialState, 0, out var initialStep))
            {
                initialSteps.Add(initialStep);
            }

            // forward pass
            Profiler.BeginSample("Forward");
            var terminalSteps = Crawl(trace, initialSteps, maxSteps);
            Profiler.EndSample();

            // backward pass
            Profiler.BeginSample("Backward");
            trace.Finalize(terminalSteps);
            Profiler.EndSample();
            return trace;
        }

        private List<Step> Crawl(Trace trace, List<Step> initialSteps, int maxSteps)
        {
            // keep track of terminals
            var terminalSteps = new List<Step>(10);

            // initialize BFS
            var queue = new Queue<Step>(1000);
            foreach (var step in initialSteps)
            {
                queue.Enqueue(step);
            }
            int visitedSteps = 0;

            // crawl!
            var stateBuilder = new StateBuilder(buffer);
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
                for (var iter = location.Transitions; iter.MoveNext();)
                {
                    var transition = iter.Current;

                    // execute transition
                    var nextLocation = transition.destinationId;
                    var result = transition.Execute(state, stateBuilder);
                    if (result == ScriptResult.Fail) continue; // transition impassable

                    // deduplicate state
                    var nextState = (result == ScriptResult.Pass) ? state : GetOrCreateState(stateBuilder.GetState());

                    // deduplicate step
                    if (trace.AddStep(nextLocation, nextState, nextDistanceFromEntrance, out var nextStep))
                    {
                        // location reached with new state -> enqueue
                        queue.Enqueue(nextStep);
                    }

                    // connect steps
                    trace.Connect(step, nextStep);
                }
            }

            return terminalSteps;
        }

        private State GetOrCreateState(State tmpState)
        {
            // deduplicate
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
    }
}
