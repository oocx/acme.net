using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Web.Administration;
using Oocx.ACME.Client;
using Oocx.ACME.Protocol;
using static Oocx.ACME.Common.Log;
using Directory = System.IO.Directory;

namespace Oocx.ACME.IIS
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class IISChallengeProvider : IChallengeProvider
    {
        private readonly AcmeClient client;
        private readonly ServerManager manager;

        public IISChallengeProvider(AcmeClient client)
        {
            this.client = client;
            manager = new ServerManager();
        }

        public async Task<PendingChallenge> AcceptChallengeAsync(string domain, string siteName, AuthorizationResponse authorization)
        {
            var challenge = authorization?.Challenges.FirstOrDefault(c => c.Type == "http-01");
            if (challenge == null)
            {
                Error("the server does not accept challenge type http-01");
                return null;
            }
            Info($"accepting challenge {challenge.Type}");

            var keyAuthorization = client.Jws.GetKeyAuthorization(challenge.Token);

            if (siteName == null)
            {
                AcceptChallengeForDomain(domain, challenge.Token, keyAuthorization);
            }
            else
            {
                AcceptChallengeForSite(siteName, challenge.Token, keyAuthorization);
            }

            return new PendingChallenge()
            {
                Instructions = $"using IIS integration to complete the challenge.",
                Complete = () => client.CompleteChallengeAsync(challenge)
            };
        }
    
    public bool CanAcceptChallengeForDomain(string domain)
        {            
            return manager.GetSiteForDomain(domain) != null;
        }
              

        public void AcceptChallengeForDomain(string domain, string token, string challengeJson)
        {
            Info($"IISChallengeService is accepting challenge with token {token} for domain {domain}");
            var root = GetIisRootAndConfigureMimeType(domain);
            CreateWellKnownDirectoryWithChallengeFile(root, token, challengeJson);                        
        }

        public void AcceptChallengeForSite(string siteName, string token, string challengeJson)
        {
            Info($"IISChallengeService is accepting challenge with token {token} for IIS web site '{siteName}'");
            var site = manager.Sites[siteName];
            if (site == null)
            {
                Error($"ISS web site '{siteName}' not found, cannot process challenge.");
                return;
            }

            var root = site.Applications["/"].VirtualDirectories["/"].PhysicalPath;
            CreateWellKnownDirectoryWithChallengeFile(root, token, challengeJson);
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

        private static void CreateWellKnownDirectoryWithChallengeFile(string root, string token, string challengeJson)
        {
            root = Environment.ExpandEnvironmentVariables(root);
            var wellKnownPath = Path.Combine(root, ".well-known");
            CreateDirectory(wellKnownPath);
            var acmePath = Path.Combine(wellKnownPath, "acme-challenge");
            CreateDirectory(acmePath);
            var challengeFilePath = Path.Combine(acmePath, token);

            Verbose($"writing challenge to {challengeFilePath}");
            File.WriteAllText(challengeFilePath, challengeJson);                        
        }

        private static void CreateDirectory(string challengePath)
        {
            if (Directory.Exists(challengePath)) return;

            Verbose($"creating directory {challengePath}");
            System.IO.Directory.CreateDirectory(challengePath);            
        }
    }
}
