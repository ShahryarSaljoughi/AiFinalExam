using System;
using System.Collections.Generic;
using System.Linq;

namespace CSP.Entities
{
    public class NodeState
    {
        public List<Variable> Variables { get; set; }

        public NodeState Clone()
        {
            var result = new NodeState();
            result.Variables = new List<Variable>();
            foreach (var variable in Variables)
            {
                result.Variables.Add(variable.Clone());
            }

            return result;
        }

        public void GoBackToState(NodeState safeState)
        {
            for (int i = 0; i < safeState.Variables.Count; i++)
            {
                Variables[i].GoBackToState(safeState.Variables[i]);
            }
        }

        public override string ToString()
        {
            return this.Variables.Any()
                ? this.Variables.Aggregate("",
                    (acc, v) =>
                        acc +
                        $"(name: {v.Name}, value: {v.AssignedValue}, isAncestorWithoutValue: {v.IsAncestorWithoutValue}, Candidates: {{{(v.Candidates.Any() ? v.Candidates.Select(c => c.Value).Aggregate((s1, s2) => $"{s1},{s2}") : string.Empty) } }} ) {Environment.NewLine}")
                : "";
        }
    }
}