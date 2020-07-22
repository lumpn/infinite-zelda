using System.Collections.Generic;
using Lumpn.Utils;

namespace Lumpn.Dungeon
{
    using Steps = Dictionary<State, Step>;

    public sealed class Location
    {
        private readonly int id;

        /// outgoing transitions
        private readonly List<Transition> transitions = new List<Transition>();

        /// steps that reached this location
        private readonly Steps steps = new Steps(StateEqualityComparer.Default);

        public int Id { get { return id; } }
        public IEnumerable<Transition> Transitions { get { return transitions; } }

        public Location(int id)
        {
            this.id = id;
        }

        public void AddTransition(Location destination, Script script)
        {
            var transition = new Transition(this, destination, script);
            transitions.Add(transition);
        }

        /// Reach this location with the specified state
        public bool AddStep(State state, int distanceFromEntrance, out Step step)
        {
            if (steps.TryGetValue(state, out step)) return false;

            step = new Step(this, state, distanceFromEntrance);
            steps.Add(state, step);
            return true;
        }

        public void Express(DotBuilder builder)
        {
            builder.AddNode(id);
            foreach (Transition transition in transitions)
            {
                transition.Express(builder);
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
    }
}
