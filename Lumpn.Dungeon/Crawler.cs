using Lumpn.Profiling;
using Lumpn.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Lumpn.Dungeon
{
    using Locations = IDictionary<int, Location>;

    public sealed class Crawler
    {
        private const int entranceId = 1;
        private const int exitId = 0;

        private readonly Locations locations;

        public Crawler(Locations locations)
        {
            this.locations = locations;
        }

        public Trace Crawl(IEnumerable<State> initialStates, int maxSteps)
        {
            var trace = new Trace();

            // find entrance & exit
            var entrance = locations.GetOrFallback(entranceId, null);
            var exit = locations.GetOrFallback(exitId, null);
            if (entrance == null) return trace;

            // initialize initial steps
            var initialSteps = new List<Step>();
            foreach (var state in initialStates)
            {
                if (trace.AddStep(entrance, state, 0, out var step))
                {
                    initialSteps.Add(step);
                }
            }

            // forward pass
            Profiler.BeginSample("Forward");
            var terminalSteps = Crawl(trace, initialSteps, maxSteps, exit);
            Profiler.EndSample();

            // backward pass
            Profiler.BeginSample("Backward");
            trace.Finalize(terminalSteps);
            Profiler.EndSample();

            return trace;
        }

        private static List<Step> Crawl(Trace trace, List<Step> initialSteps, int maxSteps, Location exit)
        {
            // keep track of terminals
            var terminalSteps = new List<Step>();

            // initialize BFS
            var queue = new Queue<Step>(initialSteps);
            int visitedSteps = 0;

            // crawl!
            while (queue.Count > 0 && (visitedSteps < maxSteps))
            {
                // fetch step
                Step step = queue.Dequeue();
                visitedSteps++;

                // track terminals
                if (step.Location == exit)
                {
                    terminalSteps.Add(step);
                }

                // try every transition
                var location = step.Location;
                var state = step.State;
                int nextDistanceFromEntrance = step.DistanceFromEntrance + 1;
                foreach (var transition in location.Transitions)
                {
                    // execute transition
                    Location nextLocation = transition.Destination;
                    State nextState = transition.Execute(state);
                    if (nextState == null) continue; // transition impassable

                    // location reached with new state -> enqueue
                    if (trace.AddStep(nextLocation, nextState, nextDistanceFromEntrance, out Step nextStep))
                    {
                        queue.Enqueue(nextStep);
                    }

                    // connect steps
                    trace.Connect(step, nextStep);
                }
            }

            return terminalSteps;
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
    }
}
