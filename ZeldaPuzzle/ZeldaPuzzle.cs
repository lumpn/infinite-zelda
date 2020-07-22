using System.Collections.Generic;
using Lumpn.Utils;
using System.Linq;

namespace Lumpn.ZeldaPuzzle
{
    public sealed class ZeldaPuzzle
    {
        public const int entranceId = 0;
        public const int exitId = 1;

        public ZeldaPuzzle(Dictionary<int, Location> locations)
        {
            this.locations = locations;
        }

        public void Crawl(List<State> initialStates, int maxSteps)
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
            Crawl(initialSteps, maxSteps);

            // initialize distance from exit
            var terminalSteps = exit.GetSteps().ToList();
            foreach (var step in terminalSteps)
            {
                step.distanceFromExit = 0;
            }

            // backward pass
            CrawlBackward(terminalSteps);
        }

        private static List<Step> Crawl(List<Step> initialSteps, int maxSteps)
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

                // try every transition
                var location = step.location;
                var state = step.state;
                int nextDistanceFromEntry = step.distanceFromEntry + 1;
                foreach (var transition in location.Transitions)
                {
                    // execute transition
                    Location nextLocation = transition.destination;
                    State nextState = transition.Execute(state);
                    if (nextState == null) continue; // transition impassable

                    // location reached with new state -> enqueue
                    if (location.AddStep(nextState, nextDistanceFromEntry, out Step nextStep))
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
                int prevDistanceFromExit = step.distanceFromExit + 1;
                foreach (var prevStep in step.Predecessors)
                {
                    if (prevStep.distanceFromExit != Step.distanceUnreachable) continue;

                    // unseen step reached -> enqueue
                    prevStep.distanceFromExit = prevDistanceFromExit;
                    queue.Enqueue(prevStep);
                }
            }
        }

        public void Express(DotBuilder builder)
        {
            var transitionBuilder = new DotTransitionBuilder();

            builder.Begin();
            foreach (var location in locations.Values)
            {
                location.Express(builder, transitionBuilder);
            }
            builder.End();
        }

        private readonly Dictionary<int, Location> locations;
    }
}
