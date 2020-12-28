using System;
using System.Collections.Generic;

namespace Lumpn.Dungeon
{
    public sealed class VariableAssignment
    {
        private readonly Dictionary<string, int> variables = new Dictionary<string, int>();

        public void Assign(string variableName, int variableValue)
        {
            variables[variableName] = variableValue;
        }

        public State ToState(VariableLookup lookup)
        {
            var stateVariables = new int[lookup.NumVariables];
            foreach (var variable in variables)
            {
                var identifier = lookup.Query(variable.Key);
                var idx = identifier.Id;
                stateVariables[idx] = variable.Value;
            }

            return new State(stateVariables);
        }
    }
}
