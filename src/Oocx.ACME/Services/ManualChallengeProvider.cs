using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Oocx.Acme.Protocol;

namespace Oocx.Acme.Services
{
    public class ManualChallengeProvider : IChallengeProvider
    {
        private readonly IAcmeClient client;

        public ManualChallengeProvider(IAcmeClient client)
        {
            this.client = client;
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

            var acmeChallengePath = System.IO.Directory.GetCurrentDirectory();
            var challengeFile = Path.Combine(acmeChallengePath, challenge.Token);
            using (var fs = new FileStream(challengeFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var data = Encoding.ASCII.GetBytes(keyAuthorization);
                await fs.WriteAsync(data, 0, data.Length);
            }

            return new PendingChallenge
            {
                Instructions = $"Copy {challengeFile} to https://{domain ?? siteName}/.well-known/acme-challenge/{challenge.Token}",
                Complete = () => client.CompleteChallengeAsync(challenge)
            };
        }
    }
}