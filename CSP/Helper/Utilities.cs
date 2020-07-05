using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSP.Entities;

namespace CSP.Helper
{
    public static class Utilities
    {
        public static Variable GetByName(this ICollection<Variable> variables, string name)
        {
            return variables.FirstOrDefault(v => v.Name == name);
        }
    }
}
