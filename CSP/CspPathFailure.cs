using System;
using CSP.Entities;

namespace CSP
{
    public class CspPathFailure : Exception
    {
        public CspPathFailure(NodeState currentState, string message = "Path Failed. You need to backtrack!") : base(
            message + $"state: {currentState.ToString()}")
        {
        }
    }
}