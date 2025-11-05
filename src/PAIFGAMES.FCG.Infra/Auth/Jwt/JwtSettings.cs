using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PAIFGAMES.FCG.Infra.Auth.Jwt.Model;

namespace PAIFGAMES.FCG.Infra.Auth.Jwt
{
    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public int Expiration { get; set; } = 1;
        public int RefreshTokenExpiration { get; set; } = 30;
        public string Issuer { get; set; } = "PAIFGAMES.FCG.Api";
        public string Audience { get; set; } = "Api";
        public RefreshTokenType RefreshTokenType { get; set; } = RefreshTokenType.Regular;
    }
    public class JwtSettingsSetup : IConfigureOptions<JwtSettings>
    {
        private const string SectionName = "Jwt";
        private readonly IConfiguration _configuration;

        public JwtSettingsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(JwtSettings options)
        {
            _configuration
                .GetSection(SectionName)
                .Bind(options);
        }
    }
}
