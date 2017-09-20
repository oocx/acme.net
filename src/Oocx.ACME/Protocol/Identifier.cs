using Newtonsoft.Json;

namespace Oocx.Acme.Protocol
{
    public class Identifier
    {
        public Identifier() { }

        public Identifier(string type, string value)
        {
            Type = type;
            Value = value;
        }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}