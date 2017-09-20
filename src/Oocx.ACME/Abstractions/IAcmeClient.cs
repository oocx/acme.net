using System.Threading.Tasks;
using Oocx.ACME.Protocol;

namespace Oocx.ACME.Services
{
    /// <summary>
    /// A client that implements the ACME protocol.
    /// https://tools.ietf.org/html/draft-ietf-acme-acme-01
    /// </summary>
    public interface IAcmeClient
    {
        /// <summary>
        /// Gets a key authorization string which is required by some ACME challenges.
        /// (See section 7.1 of the ACME specification)
        /// </summary>
        /// <seealso cref="https://tools.ietf.org/html/draft-ietf-acme-acme-01#section-7.1"/>
        /// <param name="token">The token for the challenge</param>
        /// <returns>key-authz = token || '.' || base64(JWK_Thumbprint(accountKey))</returns>
        string GetKeyAuthorization(string token);

        /// <summary>
        /// Calls the directory endpoint of the ACME server.        
        /// <seealso cref="https://tools.ietf.org/html/draft-ietf-acme-acme-01#section-6.2"/>        
        /// </summary>
        /// <returns>The endpoint URIs of the ACME server.</returns>
        Task<Directory> GetDirectoryAsync();

        /// <summary>
        /// Calls the new-reg endpoint to perform a new registration with the ACME server.
        /// <seealso cref="https://tools.ietf.org/html/draft-ietf-acme-acme-01#section-5.2"/>
        /// <seealso cref="https://tools.ietf.org/html/draft-ietf-acme-acme-01#section-6.3"/>
        /// </summary>
        /// <param name="termsOfServiceUri"> URI referring to a subscriber agreement or terms of service provided by the server. Including this field indicates the client's agreement with the referenced terms.</param>
        /// <param name="contacts">An array of URIs that the server can use to contact the client for issues related to this authorization.  For example, the server may wish to notify the client about server-initiated revocation.</param>
        /// <returns>The server's response to the registration request.</returns>
        Task<RegistrationResponse> RegisterAsync(NewRegistrationRequest request);

        /// <summary>
        /// Updates an existing registration.
        /// <seealso cref="https://tools.ietf.org/html/draft-ietf-acme-acme-01#section-5.2"/>
        /// <seealso cref="https://tools.ietf.org/html/draft-ietf-acme-acme-01#section-6.3"/>
        /// </summary>
        /// <param name="registrationUri">The uri of the registration that should be updated.</param>
        /// <param name="termsOfServiceUri"> URI referring to a subscriber agreement or terms of service provided by the server. Including this field indicates the client's agreement with the referenced terms.</param>
        /// <param name="contact">An array of URIs that the server can use to contact the client for issues related to this authorization.  For example, the server may wish to notify the client about server-initiated revocation.</param>
        /// <returns>The server's response to the update request.</returns>
        Task<RegistrationResponse> UpdateRegistrationAsync(UpdateRegistrationRequest request);

        /// <summary>
        /// Starts a dns authorization process.
        /// <seealso cref="https://tools.ietf.org/html/draft-ietf-acme-acme-01#section-6.5"/>
        /// </summary>
        /// <param name="dnsName">The domain name for which you want to request authorization.</param>        
        /// <returns></returns>
        Task<AuthorizationResponse> NewDnsAuthorizationAsync(string dnsName);

        /// <summary>
        /// Completes a challenge.
        /// <seealso cref="https://tools.ietf.org/html/draft-ietf-acme-acme-01#section-7"/>
        /// </summary>
        /// <param name="challenge">A challenge that was returned by a call to NewDnsAuthorizationAsync</param>
        /// <returns>The updated challenge object (you can check the status and error properties to see if the challenge was completed successfully.)</returns>
        Task<Challenge> CompleteChallengeAsync(Challenge challenge);

        /// <summary>
        /// Requests a new certificate.
        /// <seealso cref="https://tools.ietf.org/html/draft-ietf-acme-acme-01#section-6.6"/>
        /// </summary>
        /// <param name="csr">A DER endoced certificate signing request.</param>
        /// <returns>If the request succeeded, the response contains the DER encoded certificate.</returns>
        Task<CertificateResponse> NewCertificateRequestAsync(byte[] csr);
    }
}