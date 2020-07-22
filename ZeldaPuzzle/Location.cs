using System.Collections.Generic;
using Lumpn.Utils;

namespace Lumpn.ZeldaPuzzle
{
    public sealed class Location
    {
        public Location(int id)
        {
            this.id = id;
        }

        public void AddTransition(Location destination, ZeldaScript script)
        {
            var transition = new Transition(this, destination, script);
            transitions.Add(transition);
        }

        /// Reach this location with the specified state
        public bool AddStep(State state, int distanceFromEntry, out Step step)
        {
            if (steps.TryGetValue(state, out step)) return false;

            step = new Step(this, state, distanceFromEntry);
            steps.Add(state, step);
            return true;
        }

        public void Express(DotBuilder builder, DotTransitionBuilder transitionBuilder)
        {
            builder.AddNode(id);

            foreach (Transition transition in transitions)
            {
                transition.Express(transitionBuilder);
                transitionBuilder.Express(builder);
            }
        }

        public Step DebugGetStep(State state)
        {
            return steps.GetOrFallback(state, null);
        }

        public IEnumerable<Step> DebugGetSteps()
        {
            return steps.Values;
        }

        public int Id { get { return id; } }
        public IEnumerable<Transition> Transitions { get { return transitions; } }

        private readonly int id;

        /// outgoing transitions
        private readonly List<Transition> transitions = new List<Transition>();

        /// steps that reached this location
        private readonly Dictionary<State, Step> steps = new Dictionary<State, Step>(StateEqualityComparer.Default);
    }
}
