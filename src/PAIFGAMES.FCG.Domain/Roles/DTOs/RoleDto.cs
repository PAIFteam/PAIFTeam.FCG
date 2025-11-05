using Newtonsoft.Json;

namespace PAIFGAMES.FCG.Domain.Roles.DTOs
{
    public class RoleDto
    {
        [JsonProperty("uid")]
        public Guid UId { get; set; }
        [JsonProperty("name")]
        public string? Name { get; set; }
        [JsonProperty("concurrencyStamp")]
        public string? ConcurrencyStamp { get; set; }
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
