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

        // TODO Jonas: change to AddTransition(Location destination, Script script)?
        public void AddTransition(Transition transition)
        {
            Debug.Assert(transition.source == this);
            transitions.Add(transition);
        }

        // TODO Jonas: remove
        /// See if the location has been reached with the specified state
        public Step GetStep(State state)
        {
            return steps.GetOrFallback(state, null);
        }

        // TODO Jonas: remove
        public void GetSteps(List<Step> result)
        {
            result.AddRange(steps.Values);
        }

        // TODO Jonas: rename
        /// Reach this location with the specified state
        public Step CreateStep(State state)
        {
            Step step = new Step(this, state);
            steps.Add(state, step);
            return step;
        }

        public void Express(DotBuilder builder)
        {
            builder.AddNode(id);

            var transitionBuilder = new DotTransitionBuilder();
            foreach (Transition transition in transitions)
            {
                transition.Express(transitionBuilder);
                transitionBuilder.Express(builder);
            }
        }

        public bool Equals(Location other)
        {
            return id == other.id;
        }

        public readonly int id;

        /// outgoing transitions
        private readonly List<Transition> transitions = new List<Transition>();

        /// steps that reached this location
        private readonly Dictionary<State, Step> steps = new Dictionary<State, Step>();
    }
}
