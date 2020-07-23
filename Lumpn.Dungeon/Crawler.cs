using System.Collections.Generic;
using System.Linq;
using Lumpn.Utils;

namespace Lumpn.Dungeon
{
    using Locations = IDictionary<int, Location>;

    public sealed class Crawler
    {
        private const int entranceId = 0;
        private const int exitId = 1;

        private readonly Locations locations;

        public Crawler(Locations locations)
        {
            this.locations = locations;
        }

        public List<Step> Crawl(IEnumerable<State> initialStates, int maxSteps)
        {
            // find entrance & exit
            var entrance = locations[entranceId];
            var exit = locations[exitId];

            // initialize initial steps
            var initialSteps = new List<Step>();
            foreach (var state in initialStates)
            {
                if (entrance.AddStep(state, 0, out Step step))
                {
                    initialSteps.Add(step);
                }
            }

            // forward pass
            var terminalSteps = Crawl(initialSteps, maxSteps, exit);

            // initialize distance from exit
            foreach (var step in terminalSteps)
            {
                step.SetDistanceFromExit(0);
            }

            // backward pass
            CrawlBackward(terminalSteps);
            return terminalSteps;
        }

        private static List<Step> Crawl(List<Step> initialSteps, int maxSteps, Location exit)
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
                    if (nextLocation.AddStep(nextState, nextDistanceFromEntrance, out Step nextStep))
                    {
                        queue.Enqueue(nextStep);
                    }

                    // connect steps
                    nextStep.AddPredecessor(step);
                    step.AddSuccessor(nextStep);
                }
            }

            return terminalSteps;
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

        public void Express(DotBuilder builder)
        {
            builder.Begin();
            foreach (var location in locations.Values)
            {
                location.Express(builder);
            }
            builder.End();
        }

        public Step DebugGetStep(int locationId, State state)
        {
            var location = locations.GetOrFallback(locationId, null);
            return location?.DebugGetStep(state);
        }

        public IEnumerable<Step> DebugGetSteps()
        {
            return locations.SelectMany(p => p.Value.DebugGetSteps());
        }
    }
}
