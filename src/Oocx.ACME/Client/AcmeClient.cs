using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Oocx.ACME.Jose;
using Oocx.ACME.Protocol;
using Oocx.ACME.Services;
using Oocx.Pkcs;

using static Oocx.ACME.Logging.Log;

namespace Oocx.ACME.Client
{
    public class AcmeClient : IAcmeClient
    {
        private readonly HttpClient client;
        private Directory directory;

        private string nonce;

        private readonly JWS jws;

        public AcmeClient(HttpClient client, RSA key)
        {
            Info($"using server {client.BaseAddress}");

            this.client = client ?? throw new ArgumentNullException(nameof(client));
            jws = new JWS(key);
        }

        public AcmeClient(string baseAddress, string keyName, IKeyStore keyStore)
            : this(new HttpClient { BaseAddress = new Uri(baseAddress) }, keyStore.GetOrCreateKey(keyName))
        {
        }

        public string GetKeyAuthorization(string token) => jws.GetKeyAuthorization(token);

        public async Task<Directory> GetDirectoryAsync()
        {
            Verbose($"Querying directory information from {client.BaseAddress}");

            return await GetAsync<Directory>(new Uri("directory", UriKind.Relative)).ConfigureAwait(false);
        }

        private void RememberNonce(HttpResponseMessage response)
        {
            nonce = response.Headers.GetValues("Replay-Nonce").First();

            Verbose($"set nonce: {nonce}");
        }

        public async Task<RegistrationResponse> RegisterAsync(NewRegistrationRequest request)
        {
            await EnsureDirectoryAsync().ConfigureAwait(false);

            if (request.Agreement == null)
            {
                request.Agreement = directory.Meta.TermsOfService;
            }

            try
            {
                var registration = await PostAsync<RegistrationResponse>(directory.NewRegistration, request).ConfigureAwait(false);

                Info($"new registration created: {registration.Location}");

                return registration;
            }
            catch (AcmeException ex) when ((int)ex.Response.StatusCode == 409) // Conflict
            {
                var location = ex.Response.Headers.Location.ToString();
                Info($"using existing registration: {location}");
                var response = await PostAsync<RegistrationResponse>(new Uri(location), new UpdateRegistrationRequest()).ConfigureAwait(false);

                if (string.IsNullOrEmpty(response.Location))
                {
                    response.Location = location;
                }

                return response;
            }
        }

        public async Task EnsureDirectoryAsync()
        {
            if (directory == null || nonce == null)
            {
                directory = await GetDirectoryAsync().ConfigureAwait(false);
            }
        }

        public async Task<RegistrationResponse> UpdateRegistrationAsync(UpdateRegistrationRequest request)
        {
            await EnsureDirectoryAsync().ConfigureAwait(false);

            Info("updating registration");

            return await PostAsync<RegistrationResponse>(new Uri(request.Location), request).ConfigureAwait(false);
        }

        public async Task<AuthorizationResponse> NewDnsAuthorizationAsync(string dnsName)
        {
            await EnsureDirectoryAsync().ConfigureAwait(false);

            Info($"requesting authorization for dns identifier {dnsName}");

            var authorization = new AuthorizationRequest {
                Resource = "new-authz",
                Identifier = new Identifier("dns", dnsName)
            };

            return await PostAsync<AuthorizationResponse>(directory.NewAuthorization, authorization).ConfigureAwait(false);
        }

        public async Task<Challenge> CompleteChallengeAsync(Challenge challenge)
        {
            var challangeRequest = new KeyAuthorizationRequest {
                KeyAuthorization = jws.GetKeyAuthorization(challenge.Token)
            };

            challenge = await PostAsync<Challenge>(challenge.Uri, challangeRequest).ConfigureAwait(false);

            while ("pending".Equals(challenge?.Status, StringComparison.OrdinalIgnoreCase))
            {
                await Task.Delay(4000).ConfigureAwait(false);
                challenge = await GetAsync<Challenge>(challenge.Uri).ConfigureAwait(false);
            }

            Info($"challenge status is {challenge?.Status}");

            if (challenge?.Error?.Type != null)
            {
                Error($"{challenge.Error.Type} : {challenge.Error.Detail}");
            }

            return challenge;
        }

        public async Task<CertificateResponse> NewCertificateRequestAsync(byte[] csr)
        {
            await EnsureDirectoryAsync().ConfigureAwait(false);

            Info("requesting certificate");

            var request = new CertificateRequest {
                Csr = csr.Base64UrlEncoded()
            };

            var response = await PostAsync<CertificateResponse>(directory.NewCertificate, request).ConfigureAwait(false);

            Verbose($"location: {response.Location}");

            return response;
        }

        #region Helpers

        private async Task<TResult> GetAsync<TResult>(Uri uri) where TResult : class
        {
            return await SendAsync<TResult>(HttpMethod.Get, uri, null).ConfigureAwait(false);
        }

        private async Task<TResult> PostAsync<TResult>(Uri uri, object message) where TResult : class
        {
            return await SendAsync<TResult>(HttpMethod.Post, uri, message).ConfigureAwait(false);
        }

        private static readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented
        };

        private async Task<TResult> SendAsync<TResult>(HttpMethod method, Uri uri, object message) where TResult : class
        {
            Verbose($"{method} {uri} {message?.GetType()}");
            var nonceHeader = new AcmeHeader { Nonce = nonce };
            Verbose($"sending nonce {nonce}");

            var request = new HttpRequestMessage(method, uri);

            if (message != null)
            {
                var encodedMessage = jws.Encode(message, nonceHeader);
                var json = JsonConvert.SerializeObject(encodedMessage, jsonSettings);

                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            var response = await client.SendAsync(request).ConfigureAwait(false);

            Verbose($"response status: {(int)response.StatusCode} {response.ReasonPhrase}");

            RememberNonce(response);

            if (response.Content.Headers.ContentType.MediaType == "application/problem+json")
            {
                var problemJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var problem = JsonConvert.DeserializeObject<Problem>(problemJson);
                Verbose($"error response from server: {problem.Type}: {problem.Detail}");
                throw new AcmeException(problem, response);
            }

            if (typeof(TResult) == typeof(CertificateResponse) 
                && response.Content.Headers.ContentType.MediaType == "application/pkix-cert")
            {
                var certificateBytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

                var certificateResponse = new CertificateResponse {
                    Certificate = certificateBytes
                };

                GetHeaderValues(response, certificateResponse);

                return certificateResponse as TResult;
            }

            var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var responseContent = JObject.Parse(responseText).ToObject<TResult>();

            GetHeaderValues(response, responseContent);

            return responseContent;
        }

        private static void GetHeaderValues<TResult>(HttpResponseMessage response, TResult responseContent)
        {
            var properties =
                typeof(TResult).GetProperties(BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.Instance)
                    .Where(p => p.PropertyType == typeof(string))
                    .ToDictionary(p => p.Name, p => p);

            foreach (var header in response.Headers)
            {
                if (properties.TryGetValue(header.Key, out PropertyInfo property) 
                    && header.Value.Count() == 1)
                {
                    property.SetValue(responseContent, header.Value.First());
                }

                if (header.Key == "Link")
                {
                    foreach (var link in header.Value)
                    {
                        var parts = link.Split(';');

                        if (parts.Length != 2)
                        {
                            continue;
                        }

                        if (parts[1] == "rel=\"terms-of-service\"" && properties.ContainsKey("Agreement"))
                        {
                            properties["Agreement"].SetValue(responseContent, parts[0].Substring(1, parts[0].Length - 2));
                        }
                    }
                }
            }
        }

        #endregion
    }
}