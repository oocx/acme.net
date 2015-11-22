using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Oocx.ACME.Protocol;
using static Oocx.ACME.Common.Log;

namespace Oocx.ACME.Client
{    
    public class ManualChallengeProvider : IChallengeProvider
    {
        private readonly AcmeClient client;

        public ManualChallengeProvider(AcmeClient client)
        {
            this.client = client;
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

            var acmeChallengePath = Environment.CurrentDirectory;
            var challengeFile = Path.Combine(acmeChallengePath, challenge.Token);
            File.WriteAllText(challengeFile, keyAuthorization);

            return new PendingChallenge()
            {
                Instructions = $"Copy {challengeFile} to https://your-server/.well-known/acme/{challenge.Token}",
                Complete = () => client.CompleteChallengeAsync(challenge)
            };
        }
    }            
}