using System.Collections.Generic;

namespace Lumpn.ZeldaPuzzle
{
    /// Mutable puzzle builder
    public sealed class ZeldaPuzzleBuilder
    {
        public void AddDirectedTransition(int start, int end, ZeldaScript script)
        {
            Location source = GetOrCreateLocation(start);
            Location destination = GetOrCreateLocation(end);
            source.AddTransition(destination, script);
        }

        public void AddUndirectedTransition(int loc1, int loc2, ZeldaScript script)
        {
            AddDirectedTransition(loc1, loc2, script);
            AddDirectedTransition(loc2, loc1, script);
        }

        public void AddScript(int location, ZeldaScript script)
        {
            AddDirectedTransition(location, location, script);
        }

        public ZeldaPuzzle ToPuzzle()
        {
            return new ZeldaPuzzle(locations);
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

        private readonly Dictionary<int, Location> locations = new Dictionary<int, Location>();

        private readonly VariableLookup lookup = new VariableLookup();
    }
}
