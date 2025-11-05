using Newtonsoft.Json;

namespace PAIFGAMES.FCG.Infra.Auth.Models
{
    public class TokenRoleDto
    {
        public TokenRoleDto(string roleName, Guid roleUId)
        {
            RoleName = roleName;
            RoleUId = roleUId;
        }
        [JsonProperty("roleName")]
        public string RoleName { get; set; }
        [JsonProperty("roleUid")]
        public Guid RoleUId { get; set; }
    }
}
