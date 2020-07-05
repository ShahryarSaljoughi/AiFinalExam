using System;
using System.Collections.Generic;
using System.Linq;
using CSP.Entities;

namespace CSP
{
    public class CSPSolver
    {
        public Result Solve(IEnumerable<Variable> variables)
        {
            var initialState = new NodeState() {Variables = variables.ToList()};
            return SolveRecursively(initialState);
        }

        private Result SolveRecursively(NodeState currentState)
        {
            if (currentState.Variables.All(variable => variable.IsAssigned))
                return new Result() {AssignedVariables = currentState.Variables.ToDictionary(v => v.Name)};
            var variableToAssign = GetNextVariable(currentState);
        }

        private Variable GetNextVariable(NodeState state)
        {
            return state.Variables
                .Where(v => v.IsAssigned == false)
                .OrderByDescending(v => v.Candidates.Count())
                // todo: Degree Heuristic
                .First();
        }
    }
}