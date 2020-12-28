using System.Collections.Generic;
using System.Linq;

namespace Lumpn.Dungeon
{
    using Identifiers = Dictionary<string, VariableIdentifier>;

    public sealed class VariableLookup
    {
        private readonly Identifiers identifiers = new Identifiers();

        private int serial = 0;

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

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("(");
            foreach (var entry in identifiers.OrderBy(p => p.Value.Id))
            {
                sb.Append("\"");
                sb.Append(entry.Value.Name);
                sb.Append("\", ");
            }
            sb.Append("EOL)");

            return sb.ToString();
        }
    }
}
