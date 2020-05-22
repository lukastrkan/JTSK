using Newtonsoft.Json;

namespace JTSK.Models
{
    class ConfigModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
