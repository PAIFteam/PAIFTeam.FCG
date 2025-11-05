using Newtonsoft.Json;

namespace PAIFGAMES.FCG.Domain.Users.Filter
{
    public class UserFilterModel
    {
        [JsonProperty("search")]
        public string Search { get; set; } = string.Empty;
        [JsonProperty("roleIds")]
        public List<Guid>? RoleIds { get; set; } = new List<Guid>();
    }

}
