using System.Threading.Tasks;
using Oocx.ACME.Protocol;

namespace Oocx.ACME.Client
{
    public interface IChallengeProvider
    {
        Task<PendingChallenge> AcceptChallengeAsync(string domain, string siteName, AuthorizationResponse authorization);
    }
}