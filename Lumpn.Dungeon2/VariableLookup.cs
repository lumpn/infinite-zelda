using System.Collections.Generic;
using System.Linq;

namespace Lumpn.Dungeon2
{
    using VariableIdentifiers = Dictionary<string, VariableIdentifier>;

    public sealed class VariableLookup
    {
        private readonly VariableIdentifiers identifiers = new VariableIdentifiers();

        private int serial = 0;

        public int numVariables { get { return serial; } }

        public VariableIdentifier Unique(string name)
        {
            int uniqueId = serial++;
            return new VariableIdentifier(uniqueId, name);
        }

        public VariableIdentifier Resolve(string name)
        {
            if (!identifiers.TryGetValue(name, out var identifier))
            {
                identifier = Unique(name);
                identifiers.Add(name, identifier);
            }
            return identifier;
        }

        public VariableIdentifier Query(string name)
        {
            return identifiers[name];
        }

        public VariableIdentifier Query(int id)
        {
            return identifiers.Values.FirstOrDefault(p => p.id == id);
        }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("(Lookup (");
            sb.Append(numVariables);
            sb.Append(')');
            foreach (var entry in identifiers.Values.OrderBy(p => p.id))
            {
                sb.Append(", ");
                sb.Append(entry.id);
                sb.Append(": \"");
                sb.Append(entry.name);
                sb.Append('\"');
            }
            sb.Append(')');

            return sb.ToString();
        }
    }
}
