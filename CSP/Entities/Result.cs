using System;
using System.Collections.Generic;
using System.Linq;

namespace CSP.Entities
{
    public class Result
    {
        public IDictionary<string, Variable> AssignedVariables { get; set; }

        public override string ToString()
        {
            return AssignedVariables.Aggregate("",
                (current, targetSensors) =>
                    current + $"{targetSensors.Key}: {targetSensors.Value.AssignedValue}{Environment.NewLine}");
        }
    }
}