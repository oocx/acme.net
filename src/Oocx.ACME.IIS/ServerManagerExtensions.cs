using System;
using System.Linq;
using Microsoft.Web.Administration;

namespace Oocx.Acme.IIS
{
    public static class ServerManagerExtensions
    {
        public static Site GetSiteForDomain(this ServerManager manager, string domain)
        {
            return manager.Sites.SingleOrDefault(s => s.Bindings.Any(b => string.Equals(domain, b.Host, StringComparison.OrdinalIgnoreCase)));
        }
    }
}