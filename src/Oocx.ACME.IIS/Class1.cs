using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Web.Administration;


namespace Oocx.ACME.IIS
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class IisIntegration
    {
        private readonly ServerManager manager;

        public IisIntegration()
        {
            manager = new ServerManager();
        }
        public bool CanAcceptChallengeForDomain(string domain)
        {            
            return GetSiteForDomain(domain) != null;
        }

        private Site GetSiteForDomain(string domain)
        {
            return manager.Sites.SingleOrDefault(s => s.Bindings.Any(b => string.Equals(domain, b.Host, StringComparison.OrdinalIgnoreCase)));
        }

        public void AcceptChallenge(string domain, string token, string challengeJson)
        {
            var challengePath = Path.Combine(Path.GetTempPath(), "oocx.acme");
            CreateDirectory(challengePath);
            var wellKnownPath = Path.Combine(challengePath, ".well-known");
            CreateDirectory(wellKnownPath);
            var acmePath = Path.Combine(wellKnownPath, "acme");
            CreateDirectory(acmePath);
            var challengeFilePath = Path.Combine(acmePath, token);
            File.WriteAllText(challengeFilePath, challengeJson);

            var site = GetSiteForDomain(domain);
            var root = site.Applications["/"];
            var wellKnownDir = root.VirtualDirectories.FirstOrDefault(d => "/.well-known".Equals(d.Path));
            if (wellKnownDir == null)
            {
                var wellKnownVdir = root.VirtualDirectories.Add(".well-known", wellKnownPath);
                root.VirtualDirectories.Add(".well-known/acme", acmePath);
            }

        }

        private static void CreateDirectory(string challengePath)
        {
            if (!System.IO.Directory.Exists(challengePath))
            {
                System.IO.Directory.CreateDirectory(challengePath);
            }
        }
    }
}
