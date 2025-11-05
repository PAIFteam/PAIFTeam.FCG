using Newtonsoft.Json;
using PAIFGAMES.FCG.Infra.Auth.Models;

namespace PAIFGAMES.FCG.Infra.Auth.Users
{
    public class UserToken
    {
        public UserToken()
        {
        }

        public UserToken(Guid id, string fullName, string email, List<TokenRoleDto>? roles = null)
        {
            UId = id;
            FullName = fullName;
            Email = email;
            Roles = roles;
        }

        [JsonProperty("id")]
        public Guid UId { get; set; }

        [JsonProperty("name")]
        public string FullName { get; set; }

        [JsonProperty("login")]
        public string Email { get; set; }

        [JsonProperty("roles")]
        public List<TokenRoleDto>? Roles { get; set; }
    }
}
