using System.Collections.Generic;

namespace CSP.Entities
{
    public class NodeState
    {
        public List<Variable> Variables { get; set; }

        public NodeState Clone()
        {
            var result = new NodeState();
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
    }
}