using Newtonsoft.Json;

namespace JTSK.Models
{
    class ConfigModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
