using Newtonsoft.Json;

namespace PAIFGAMES.FCG.Domain.Games.DTOs
{
    public class GameDto
    {
        [JsonProperty("uid")]
        public Guid UId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
