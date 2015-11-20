using Newtonsoft.Json;

namespace Oocx.ACME.Protocol
{
    public class Error
    {

        [JsonProperty("detail")]
        public string Detail { get; set; }


        [JsonProperty("type")]
        public string Type { get; set; }
    }
}