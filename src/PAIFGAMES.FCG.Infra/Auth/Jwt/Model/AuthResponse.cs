using Newtonsoft.Json;
using PAIFGAMES.FCG.Infra.Auth.Users;

namespace PAIFGAMES.FCG.Infra.Auth.Jwt.Model
{
    public class AuthResponse
    {
        [JsonProperty("acessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("expiresIn")]
        public double ExpiresIn { get; set; }

        [JsonProperty("userToken")]
        public UserToken UserToken { get; set; }

        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
    }
}
