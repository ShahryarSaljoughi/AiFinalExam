using System;

namespace CSP
{
    public class CspPathFailure: Exception
    {
        public override string Message => "Path Failed. You need to backtrack!";
    }
}