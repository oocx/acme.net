using System;
using System.Net.Http;

using Oocx.Acme.Protocol;

namespace Oocx.Acme
{
    public class AcmeException : Exception
    {
        public AcmeException(Problem problem, HttpResponseMessage response)
           : base($"{problem.Type}: {problem.Detail}")
        {
            Problem = problem;
            Response = response;
        }

        public Problem Problem { get; }

        public HttpResponseMessage Response { get; }
    }
}