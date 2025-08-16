using System;
using System.Collections.Generic;
using System.Linq;

namespace Lumpn.Dungeon
{
    public sealed class CrawlerBuilder
    {
        private enum TransitionType
        {
            Script,
            Directed,
            Undirected,
        }

        private struct Transition
        {
            public TransitionType type;
            public int start, end;
            public Script script;

            public void Express(DotBuilder builder)
            {
                switch (type)
                {
                    case TransitionType.Script:
                    case TransitionType.Directed:
                        builder.AddEdge(start, end, script.Name);
                        break;
                    case TransitionType.Undirected:
                        builder.AddUndirectedEdge(start, end, script.Name);
                        break;
                }
            }
        }

        private readonly List<Transition> transitions = new List<Transition>();

        public void AddDirectedTransition(int start, int end, Script script)
        {
            var transition = new Transition
            {
                type = TransitionType.Directed,
                start = start,
                end = end,
                script = script,
            };
            transitions.Add(transition);
        }

        public void AddUndirectedTransition(int loc1, int loc2, Script script)
        {
            var transition = new Transition
            {
                type = TransitionType.Undirected,
                start = loc1,
                end = loc2,
                script = script,
            };
            transitions.Add(transition);
        }

        public void AddScript(int location, Script script)
        {
            var transition = new Transition
            {
                type = TransitionType.Script,
                start = location,
                end = location,
                script = script,
            };
            transitions.Add(transition);
        }

        public Crawler Build()
        {
            var locations = new Dictionary<int, Location>();
            foreach (var transition in transitions)
            {
                var source = GetOrCreateLocation(transition.start, locations);
                var destination = GetOrCreateLocation(transition.end, locations);

                source.AddTransition(destination, transition.script);
                if (transition.type == TransitionType.Undirected)
                {
                    destination.AddTransition(source, transition.script);
                }
            }
            return new Crawler(locations);
        }

        public void Express(DotBuilder builder)
        {
            var starts = transitions.Select(p => p.start);
            var ends = transitions.Select(p => p.end);
            var nodes = starts.Concat(ends).Distinct().OrderBy(p => p);

            builder.Begin();
            foreach (var node in nodes)
            {
                builder.AddNode(node);
            }
            foreach (var transition in transitions)
            {
                transition.Express(builder);
            }
            builder.End();
        }

        private static Location GetOrCreateLocation(int id, IDictionary<int, Location> locations)
        {
            if (!locations.TryGetValue(id, out Location location))
            {
                location = new Location(id);
                locations.Add(id, location);
            }
            return location;
        }
    }
}
