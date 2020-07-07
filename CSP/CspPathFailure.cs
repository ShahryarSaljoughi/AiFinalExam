using System;

namespace CSP
{
    public class CspPathFailure: Exception
    {
        public CspPathFailure(string message): base(message ?? "Path Failed. You need to backtrack!")
        {
            
        }
    }
}