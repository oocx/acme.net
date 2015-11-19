using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
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
            var wellKnownPath = CreateWellKnownDirectory(token, challengeJson);            
            CreateIISVirtualDirectory(domain, wellKnownPath);
        }

        private static void AllowReadPermissionsForEveryone(string wellKnownPath)
        {
            //TODO instead of allowing read for everyone, only allow read for the IIS application pool account
            var accessControl = File.GetAccessControl(wellKnownPath);
            var everyone = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
            var allowRead = new FileSystemAccessRule(everyone, FileSystemRights.Read, AccessControlType.Allow);
            accessControl.AddAccessRule(allowRead);
            File.SetAccessControl(wellKnownPath, accessControl);
        }

        private void CreateIISVirtualDirectory(string domain, string wellKnownPath)
        {
            var site = GetSiteForDomain(domain);
            var root = site.Applications["/"];
            var wellKnownDir = root.VirtualDirectories.FirstOrDefault(d => "/.well-known".Equals(d.Path));
            if (wellKnownDir == null)
            {
                Verbose("creating virtual directory /.well-known/acme");
                var wellKnownVdir = root.VirtualDirectories.Add("/.well-known", wellKnownPath);
                var acmeVdir = root.VirtualDirectories.Add("/.well-known/acme", Path.Combine(wellKnownPath, "acme"));
                manager.CommitChanges();
            }
        }

        private static string CreateWellKnownDirectory(string token, string challengeJson)
        {
            var challengePath = Path.Combine(Path.GetTempPath(), "oocx.acme");
            CreateDirectory(challengePath);
            var wellKnownPath = Path.Combine(challengePath, ".well-known");
            if (CreateDirectory(wellKnownPath)) AllowReadPermissionsForEveryone(wellKnownPath); ;
            var acmePath = Path.Combine(wellKnownPath, "acme");
            CreateDirectory(acmePath);
            var challengeFilePath = Path.Combine(acmePath, token);
            File.WriteAllText(challengeFilePath, challengeJson);
            var acmeWebConfigFilePath = Path.Combine(acmePath, "web.config");
            File.WriteAllText(acmeWebConfigFilePath, AcmeWebConfigContents);
            return wellKnownPath;
        }

        private static bool CreateDirectory(string challengePath)
        {
            if (Directory.Exists(challengePath)) return false;

            Verbose($"creating directory {challengePath}");
            System.IO.Directory.CreateDirectory(challengePath);
            return true;
        }
    }
}
