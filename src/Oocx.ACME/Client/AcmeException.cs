using System;
using Oocx.ACME.Protocol;

namespace Oocx.ACME.Client
{
    public class AcmeException : Exception
    {
        public Problem Problem { get; }

        public AcmeException(Problem problem) : base($"{problem.Type}: {problem.Detail}")
        {
            Problem = problem;
        }
    }
}