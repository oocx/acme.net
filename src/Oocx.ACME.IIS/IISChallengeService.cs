using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Web.Administration;
using static Oocx.ACME.Common.Log;

namespace Oocx.ACME.IIS
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class IISChallengeService
    {
        private readonly ServerManager manager;

        public IISChallengeService()
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

        const string AcmeWebConfigContents =
            "<?xml version = \"1.0\" encoding=\"UTF-8\"?><configuration><system.webServer><staticContent><mimeMap fileExtension = \".\" mimeType=\"application/jose+json\" /></staticContent></system.webServer></configuration>";

        public void AcceptChallenge(string domain, string token, string challengeJson)
        {
            Info($"IISChallengeService is accepting challenge with token {token} for domain {domain}");
            var challengePath = Path.Combine(Path.GetTempPath(), "oocx.acme");
            CreateDirectory(challengePath);
            var wellKnownPath = Path.Combine(challengePath, ".well-known");
            CreateDirectory(wellKnownPath);
            var acmePath = Path.Combine(wellKnownPath, "acme");
            CreateDirectory(acmePath);
            var challengeFilePath = Path.Combine(acmePath, token);
            File.WriteAllText(challengeFilePath, challengeJson);
            var acmeWebConfigFilePath = Path.Combine(acmePath, "web.config");
            File.WriteAllText(acmeWebConfigFilePath, AcmeWebConfigContents);

            var site = GetSiteForDomain(domain);
            var root = site.Applications["/"];
            var wellKnownDir = root.VirtualDirectories.FirstOrDefault(d => "/.well-known".Equals(d.Path));
            if (wellKnownDir == null)
            {
                Verbose("creating virtual directory /.well-known/acme");
                var wellKnownVdir = root.VirtualDirectories.Add("/.well-known", wellKnownPath);                
                var acmeVdir = root.VirtualDirectories.Add("/.well-known/acme", acmePath);
                manager.CommitChanges();
            }           
            
             
        }

        private static void CreateDirectory(string challengePath)
        {
            if (!System.IO.Directory.Exists(challengePath))
            {
                Verbose($"creating directory {challengePath}");
                System.IO.Directory.CreateDirectory(challengePath);
            }
        }
    }
}
