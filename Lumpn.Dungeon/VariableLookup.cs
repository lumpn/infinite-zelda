using System.Collections.Generic;
using System.Linq;

namespace Lumpn.Dungeon
{
    using NamedIdentifiers = Dictionary<string, VariableIdentifier>;

    public sealed class VariableLookup
    {
        private readonly List<VariableIdentifier> identifiers = new List<VariableIdentifier>();
        private readonly NamedIdentifiers namedIdentifiers = new NamedIdentifiers();

        public int NumVariables { get { return identifiers.Count; } }

        public VariableIdentifier Unique(string name)
        {
            int uniqueId = identifiers.Count;
            var identifier = new VariableIdentifier(uniqueId, name);
            identifiers.Add(identifier);
            return identifier;
        }

        public VariableIdentifier Resolve(string name)
        {
            if (!namedIdentifiers.TryGetValue(name, out VariableIdentifier identifier))
            {
                identifier = Unique(name);
                namedIdentifiers.Add(name, identifier);
            }
            return identifier;
        }

        public VariableIdentifier Query(string name)
        {
            return namedIdentifiers[name];
        }

        public VariableIdentifier Query(int id)
        {
            return identifiers[id];
        }

        public VariableIdentifier QueryNamed(int id)
        {
            return namedIdentifiers.Values.FirstOrDefault(p => p.Id == id);
        }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("(Lookup (");
            sb.Append(NumVariables);
            sb.Append(")");
            foreach (var entry in identifiers)
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
