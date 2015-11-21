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
            return manager.GetSiteForDomain(domain) != null;
        }
              

        public void AcceptChallenge(string domain, string token, string challengeJson)
        {
            Info($"IISChallengeService is accepting challenge with token {token} for domain {domain}");
            var root = GetIisRootAndConfigureMimeType(domain);
            var wellKnownPath = CreateWellKnownDirectory(root, token, challengeJson);            
            
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

        private string GetIisRootAndConfigureMimeType(string domain)
        {
            var site = manager.GetSiteForDomain(domain);

            var app = site.Applications["/"];

            var config = app.GetWebConfiguration();
            var staticContent = config.GetSection("system.webServer/staticContent");
            var collection = staticContent.GetCollection();
            if (!collection.Any(
                    e => e.ElementTagName == "mimeMap" && ".".Equals(e.GetAttribute("fileExtension").Value)))
            {
                Info("adding mime type 'text/plain' for extension '.'");
                var mime = collection.CreateElement("mimeMap");
                mime["fileExtension"] = ".";
                mime["mimeType"] = "text/plain";
                collection.Add(mime);
                manager.CommitChanges();
            }
            else
            {
                Info("configuration already contains mime type mapping for extension '.'");
            }                        

            return app.VirtualDirectories["/"].PhysicalPath;
        }

        private static string CreateWellKnownDirectory(string root, string token, string challengeJson)
        {
            root = Environment.ExpandEnvironmentVariables(root);
            var wellKnownPath = Path.Combine(root, ".well-known");
            CreateDirectory(wellKnownPath);
            var acmePath = Path.Combine(wellKnownPath, "acme-challenge");
            CreateDirectory(acmePath);
            var challengeFilePath = Path.Combine(acmePath, token);
            File.WriteAllText(challengeFilePath, challengeJson);
            
            return wellKnownPath;
        }

        private static void CreateDirectory(string challengePath)
        {
            if (Directory.Exists(challengePath)) return;

            Verbose($"creating directory {challengePath}");
            System.IO.Directory.CreateDirectory(challengePath);            
        }
    }
}
