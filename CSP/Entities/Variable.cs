using System.Collections.Generic;

namespace CSP.Entities
{
    public class Variable
    {
        public string Name { get; set; }
        public string AssignedValue { get; set; }
        public bool IsAssigned { get; set; }
        public IEnumerable<string> Candidates { get; set; }

    }
}