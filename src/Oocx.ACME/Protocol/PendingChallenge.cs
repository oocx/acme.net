using System;
using System.Threading.Tasks;

namespace Oocx.ACME.Protocol
{
    public class PendingChallenge
    {
        public string Instructions { get; set; }

        public Func<Task<Challenge>> Complete { get; set; }
    }
}