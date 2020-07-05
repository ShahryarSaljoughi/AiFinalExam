using System.Collections.Generic;

namespace CSP.Entities
{
    public class Variable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AssignedValue { get; set; }
        public bool IsAssigned { get; set; }
        public IEnumerable<DomainValue> Candidates { get; set; }

        public Variable Clone()
        {
            return new Variable()
            {
                Name = this.Name,
                AssignedValue = this.AssignedValue,
                IsAssigned =  this.IsAssigned,
                Candidates = this.Candidates
            };
        }

        public void GoBackToState(Variable safeStateVariable)
        {
            Name = safeStateVariable.Name;
            AssignedValue = safeStateVariable.AssignedValue;
            IsAssigned = safeStateVariable.IsAssigned;
            Candidates = safeStateVariable.Candidates;
        }
    }
}