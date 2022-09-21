using System.Collections.Generic;
using System.Linq;

namespace Lumpn.Dungeon
{
    using Identifiers = Dictionary<string, VariableIdentifier>;

    public sealed class VariableLookup
    {
        private readonly Identifiers identifiers = new Identifiers();

        private int serial = 0;

        public int NumVariables { get { return serial; } }

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
            return identifiers.Values.FirstOrDefault(p => p.Id == id);
        }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("(Lookup (");
            sb.Append(NumVariables);
            sb.Append(")");
            foreach (var entry in identifiers.Values.OrderBy(p => p.Id))
            {
                sb.Append(", ");
                sb.Append(entry.Id);
                sb.Append(": \"");
                sb.Append(entry.Name);
                sb.Append("\"");
            }
            sb.Append(")");

            return sb.ToString();
        }
    }
}
