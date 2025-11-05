using Newtonsoft.Json;
using PAIFGAMES.FCG.Domain.Games.DTOs;

namespace PAIFGAMES.FCG.Domain.Users.DTOs
{
    public class UserDto
    {
        [JsonProperty("uid")]
        public Guid UId { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }
        [JsonProperty("userName")]
        public string UserName { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("lockoutEnd")]
        public DateTimeOffset? LockoutEnd { get; set; }
        [JsonProperty("lockoutEnabled")]
        public bool LockoutEnabled { get; set; }
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("roles")]
        public IEnumerable<RoleInfo>? Roles { get; set; } = new List<RoleInfo>();

        [JsonProperty("games")]
        public IEnumerable<GameDto>? Games { get; set; } = new List<GameDto>();
    }

    public class RoleInfo
    {
        [JsonProperty("roleName")]
        public string RoleName { get; set; }
        [JsonProperty("roleUId")]
        public Guid RoleUId { get; set; }

    }
}
