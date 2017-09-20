using System;
using Newtonsoft.Json;

namespace Oocx.Acme.Protocol
{
    public class Challenge
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// The URI to which a response can be posted.
        /// </summary>
        [JsonProperty("uri")]
        public Uri Uri { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        /// <summary>
        /// The status of this authorization.
        /// Possible values are: "unknown", "pending", "processing", "valid", "invalid" and "revoked". 
        /// If this field is missing, then the default value is "pending".
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// The error that occurred while the server was validating the challenge.
        /// </summary>
        [JsonProperty("error")]
        public Error Error { get; set; }

        [JsonProperty("tls")]
        public bool Tls { get; set; }

        /// <summary>
        /// The time at which this challenge was completed by the server
        /// </summary>
        [JsonProperty("validated")]
        public DateTime? Validated { get; set; }

        [JsonProperty("keyAuthorization")]
        public string KeyAuthorization { get; set; }

        [JsonProperty("validationRecord")]
        public ValidationRecord[] ValidationRecord { get; set; }
    }
}