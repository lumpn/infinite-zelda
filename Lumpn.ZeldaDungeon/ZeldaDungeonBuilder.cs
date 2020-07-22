using System.Collections.Generic;
using Lumpn.Dungeon;

namespace Lumpn.ZeldaDungeon
{
    using Locations = Dictionary<int,Location>;

    public sealed class ZeldaDungeonBuilder
    {
        private readonly Locations locations = new Locations();

        private readonly VariableLookup lookup = new VariableLookup();

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
            return new Crawler(locations);
        }

        private Location GetOrCreateLocation(int id)
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
