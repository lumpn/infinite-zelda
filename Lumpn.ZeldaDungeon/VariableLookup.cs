using System.Collections.Generic;

namespace Lumpn.ZeldaPuzzle
{
    public sealed class VariableLookup
    {
        public VariableIdentifier Unique(string name)
        {
            int uniqueId = serial++;
            return new VariableIdentifier(uniqueId, name);
        }

        public VariableIdentifier Resolve(string name)
        {
            if (!identifiers.TryGetValue(name, out VariableIdentifier identifier))
            {
                identifier = Unique(name);
                identifiers[name] = identifier;
            }
            return identifier;
        }

        private int serial = 0;

        private readonly Dictionary<string, VariableIdentifier> identifiers = new Dictionary<string, VariableIdentifier>();
    }
}
