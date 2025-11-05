using Newtonsoft.Json;

namespace PAIFGAMES.FCG.Domain.Games.Filter
{
    public class GameFilterModel
    {
        [JsonProperty("search")]
        public string Search { get; set; } = string.Empty;
        [JsonProperty("requiredParams")]
        public List<string> RequiredParams { get; set; } = new List<string>();
    }
}
