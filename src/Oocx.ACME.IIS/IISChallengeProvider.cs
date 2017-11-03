using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Web.Administration;
using Oocx.Acme.Services;
using Oocx.Acme.Protocol;
using Directory = System.IO.Directory;

namespace Oocx.Acme.IIS
{
    public class IISChallengeProvider : IChallengeProvider
    {
        private readonly IAcmeClient client;
        private readonly ServerManager manager = new ServerManager();

        public IISChallengeProvider(IAcmeClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<PendingChallenge> AcceptChallengeAsync(string domain, string siteName, AuthorizationResponse authorization)
        {
            var challenge = authorization?.Challenges.FirstOrDefault(c => c.Type == "http-01");

            if (challenge == null)
            {
                Log.Error("the server does not accept challenge type http-01");
                return null;
            }
            Log.Info($"accepting challenge {challenge.Type}");

            var keyAuthorization = client.GetKeyAuthorization(challenge.Token);

            if (siteName == null)
            {
                await AcceptChallengeForDomainAsync(domain, challenge.Token, keyAuthorization);
            }
            else
            {
                await AcceptChallengeForSiteAsync(siteName, challenge.Token, keyAuthorization);
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

        public async Task AcceptChallengeForDomainAsync(string domain, string token, string challengeJson)
        {
            Log.Info($"IISChallengeService is accepting challenge with token {token} for domain {domain}");
            var root = GetIisRoot(domain);
            await CreateWellKnownDirectoryWithChallengeFileAsync(root, token, challengeJson);
        }

        public async Task AcceptChallengeForSiteAsync(string siteName, string token, string challengeJson)
        {
            Log.Info($"IISChallengeService is accepting challenge with token {token} for IIS web site '{siteName}'");
            var site = manager.Sites[siteName];
            if (site == null)
            {
                Log.Error($"IIS web site '{siteName}' not found, cannot process challenge.");
                return;
            }

            var root = site.Applications["/"].VirtualDirectories["/"].PhysicalPath;
            await CreateWellKnownDirectoryWithChallengeFileAsync(root, token, challengeJson);
        }

        private string GetIisRoot(string domain)
        {
            var site = manager.GetSiteForDomain(domain);
            var app = site.Applications["/"];
            return app.VirtualDirectories["/"].PhysicalPath;
        }

        private static async Task CreateWellKnownDirectoryWithChallengeFileAsync(string root, string token, string keyAuthorization)
        {
            root = Environment.ExpandEnvironmentVariables(root);
            var wellKnownPath = Path.Combine(root, ".well-known");
            CreateDirectory(wellKnownPath);
            var acmePath = Path.Combine(wellKnownPath, "acme-challenge");
            CreateDirectory(acmePath);

            CreateWebConfig(acmePath);

            var challengeFilePath = Path.Combine(acmePath, token);

            Log.Verbose($"writing challenge to {challengeFilePath}");
            using (var fs = new FileStream(challengeFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var data = System.Text.Encoding.ASCII.GetBytes(keyAuthorization);
                await fs.WriteAsync(data, 0, data.Length);
            }
        }

        private static void CreateWebConfig(string acmePath)
        {
            var webConfigPath = Path.Combine(acmePath, "web.config");

            if (File.Exists(webConfigPath))
            {
                return;
            }

            Type iisChallengeProviderType = typeof(IISChallengeProvider);

            string resourceName = iisChallengeProviderType.ToString().Substring(0, iisChallengeProviderType.ToString().LastIndexOf(".")) + ".web.config";
            Log.Verbose($"Creating file '{webConfigPath}' from internal resource '{resourceName}'");

            var webConfigStream = iisChallengeProviderType.GetTypeInfo().Assembly.GetManifestResourceStream(resourceName);

            using (var fileStream = new FileStream(webConfigPath, FileMode.CreateNew, FileAccess.Write))
            {
                webConfigStream.CopyTo(fileStream);
                webConfigStream.Flush();
                webConfigStream.Close();
            }
        }

        private static void CreateDirectory(string challengePath)
        {
            if (Directory.Exists(challengePath)) return;

            Log.Verbose($"creating directory {challengePath}");

            Directory.CreateDirectory(challengePath);
        }
    }
}
