using Newtonsoft.Json;
using PAIFGAMES.FCG.Infra.Auth.Models;

namespace PAIFGAMES.FCG.Infra.Auth.Users
{
    public class UserData
    {
        [JsonProperty("name")]
        public string FullName { get; set; }

        [JsonProperty("unique_name")]
        public string Username { get; set; }

        [JsonProperty("user")]
        public UserToken User { get; set; }

        [JsonProperty("roles")]
        public List<TokenRoleDto>? Roles { get; set; }
    }
}
