using System.Collections.Generic;

namespace Lumpn.Dungeon2
{
    public sealed class Location
    {
        private readonly int id;

        /// outgoing transitions
        private readonly List<Transition> transitions = new List<Transition>();

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

        public void Express(DotBuilder builder)
        {
            builder.AddNode(id);
            foreach (var transition in transitions)
            {
                transition.Express(builder);
            }
        }

        public override string ToString()
        {
            return id.ToString();
        }
    }
}
