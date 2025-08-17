using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

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

            public void Express(DotBuilder builder, Script noOp, int transitionId)
            {
                if (string.IsNullOrWhiteSpace(script.Name))
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
                else
                {
                    var shape = (type == TransitionType.Script) ? "diamond" : "box";
                    builder.AddNode(transitionId, $"{script.Name}\", shape=\"{shape}");
                    switch (type)
                    {
                        case TransitionType.Script:
                            builder.AddUndirectedEdge(start, transitionId, string.Empty);
                            break;
                        case TransitionType.Directed:
                            builder.AddEdge(start, transitionId, string.Empty);
                            builder.AddEdge(transitionId, end, string.Empty);
                            break;
                        case TransitionType.Undirected:
                            builder.AddUndirectedEdge(start, transitionId, string.Empty);
                            builder.AddUndirectedEdge(transitionId, end, string.Empty);
                            break;
                    }
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

        public void MergeNodes(Script noOp)
        {
            while (TryFindMergeTransition(noOp, out int a, out int b))
            {
                for (int i = transitions.Count - 1; i >= 0; i--)
                {
                    var transition = transitions[i];
                    if (transition.start == b)
                    {
                        transition.start = a;
                    }
                    if (transition.end == b)
                    {
                        transition.end = a;
                    }
                    transitions[i] = transition;

                    if (transition.script == noOp && transition.start == transition.end)
                    {
                        transitions.RemoveAt(i);
                    }
                }
            }
        }

        private bool TryFindMergeTransition(Script noOp, out int nodeA, out int nodeB)
        {
            foreach (var transition in transitions)
            {
                if (transition.script == noOp && transition.start != transition.end && transition.type == TransitionType.Undirected)
                {
                    nodeA = transition.start;
                    nodeB = transition.end;
                    if (nodeB == 0 || nodeB == 1)
                    {
                        var tmp = nodeA;
                        nodeA = nodeB;
                        nodeB = tmp;
                    }
                    return true;
                }
            }
            nodeA = 0;
            nodeB = 0;
            return false;
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

        public void Express(DotBuilder builder, Script noOp)
        {
            var starts = transitions.Select(p => p.start);
            var ends = transitions.Select(p => p.end);
            var nodes = starts.Concat(ends).Distinct().OrderBy(p => p);

            var transitionId = nodes.DefaultIfEmpty(0).Max();
            builder.Begin();
            foreach (var node in nodes)
            {
                builder.AddNode(node);
            }
            foreach (var transition in transitions)
            {
                transition.Express(builder, noOp, ++transitionId);
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
