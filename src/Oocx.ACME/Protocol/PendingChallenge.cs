using System;

namespace Oocx.ACME.Protocol
{
    public class PendingChallenge
    {
        public string Instructions { get; set; }

        public Action Complete { get; set; }
    }
}