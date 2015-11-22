using System.Threading.Tasks;
using Oocx.ACME.Protocol;

namespace Oocx.ACME.Client
{
    public interface IAcmeClient
    {
        string GetKeyAuthorization(string token);
        Task<Directory> DiscoverAsync();
        Task<RegistrationResponse> RegisterAsync(bool acceptTermsOfService);
        Task EnsureDirectory();
        Task<RegistrationResponse> UpdateRegistrationAsync(string registrationUri, string agreementUri);
        Task<AuthorizationResponse> NewDnsAuthorizationAsync(string dnsName);
        Task<Challenge> CompleteChallengeAsync(Challenge challenge);
        Task<CertificateResponse> NewCertificateRequestAsync(byte[] csr);
    }
}