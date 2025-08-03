using System.Collections.Generic;

namespace Lumpn.Dungeon2
{
    using Locations = Dictionary<int, Location>;

    public sealed class CrawlerBuilder
    {
        private readonly Locations locations = new Locations();

        private readonly VariableLookup lookup = new VariableLookup();

        public VariableLookup Lookup { get { return lookup; } }

        public void AddDirectedTransition(int start, int end, Script script)
        {
            Location source = GetOrCreateLocation(start);
            Location destination = GetOrCreateLocation(end);
            source.AddTransition(destination, script);
        }

        public void AddUndirectedTransition(int loc1, int loc2, Script script)
        {
            AddDirectedTransition(loc1, loc2, script);
            AddDirectedTransition(loc2, loc1, script);
        }

        public void AddScript(int location, Script script)
        {
            AddDirectedTransition(location, location, script);
        }

        public Crawler Build()
        {
            var buffer = new Memory(lookup.numVariables, 1);
            return new Crawler(locations, lookup.numVariables, buffer);
        }

        private Location GetOrCreateLocation(int id)
        {
            if (!locations.TryGetValue(id, out var location))
            {
                location = new Location(id);
                locations.Add(id, location);
            }
            return location;
        }
    }
}
