using System.Collections.Generic;
using System.Linq;

namespace CSP.Entities
{
    public class Variable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AssignedValue { get; set; }
        public bool IsAssigned { get; set; }
        public IEnumerable<DomainValue> Candidates { get; set; }
        public bool IsAncestorWithoutValue { get; set; }

        public Variable Clone()
        {
            var result =
                new Variable()
                {
                    Name = this.Name,
                    AssignedValue = this.AssignedValue,
                    IsAssigned = this.IsAssigned,
                    Id = this.Id,
                    IsAncestorWithoutValue = IsAncestorWithoutValue
                };
            result.Candidates = this.Candidates.ToArray();
            return result;
        }

        public void GoBackToState(Variable safeStateVariable)
        {
            Name = safeStateVariable.Name;
            AssignedValue = safeStateVariable.AssignedValue;
            IsAssigned = safeStateVariable.IsAssigned;
            Candidates = safeStateVariable.Candidates;
            IsAncestorWithoutValue = safeStateVariable.IsAncestorWithoutValue;
        }

        public void RemoveFromCandidateList(DomainValue domainValue)
        {
            this.Candidates = this.Candidates.Where(c => c.Value != domainValue.Value);
        }
    }
}