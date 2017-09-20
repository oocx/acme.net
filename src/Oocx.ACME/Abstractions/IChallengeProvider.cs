using System.Threading.Tasks;
using Oocx.Acme.Protocol;

namespace Oocx.Acme.Services
{
    public interface IChallengeProvider
    {
        Task<PendingChallenge> AcceptChallengeAsync(string domain, string siteName, AuthorizationResponse authorization);
    }
}