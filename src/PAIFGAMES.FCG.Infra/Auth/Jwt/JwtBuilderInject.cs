using PAIFGAMES.FCG.Infra.Auth.Interfaces;
using PAIFGAMES.FCG.Infra.Auth.Jwt.Model;
using PAIFGAMES.FCG.Domain.Entities;
using PAIFGAMES.FCG.Infra.Auth.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json;
using JsonClaimValueTypes = Microsoft.IdentityModel.JsonWebTokens.JsonClaimValueTypes;
using System.Text.Json;
using PAIFGAMES.FCG.Infra.Auth.Models;

namespace PAIFGAMES.FCG.Infra.Auth.Jwt
{
    public class JwtBuilderInject<TIdentityUser, TKey> : IJwtBuilder
        where TIdentityUser : IdentityUserCustom
        where TKey : IEquatable<TKey>
    {
        private readonly UserManager<TIdentityUser> _userManager;
        private readonly RoleManager<IdentityRoleCustom> _roleManager;
        private readonly IOptions<JwtSettings> _settings;
        private CancellationToken _cancellationToken;

        private ClaimsIdentity _identityClaimsAccess;
        private ClaimsIdentity _identityClaimsRefresh;
        private TIdentityUser? _user;

        private bool _useEmail = false;
        private bool _useClaims = false;

        private string _email;
        private List<TokenRoleDto>? _userRoles;
        private UserData _userData = new UserData();
        private UserToken _userResponse = new UserToken();


        public JwtBuilderInject(
            UserManager<TIdentityUser> userManager,
            RoleManager<IdentityRoleCustom> roleManager,
            IOptions<JwtSettings> settings
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _settings = settings;
            _identityClaimsAccess = new ClaimsIdentity();
            _identityClaimsRefresh = new ClaimsIdentity();
        }

        public IJwtBuilder WithEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException(nameof(email));

            _useEmail = true;
            _email = email;

            return this;
        }

        public IJwtBuilder WithUserClaims()
        {
            _useClaims = true;

            return this;
        }

        public async Task<AuthResponse> BuildUserResponse(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            var user = new AuthResponse
            {
                AccessToken = await BuildToken(),
                ExpiresIn = TimeSpan.FromSeconds(_settings.Value.Expiration).TotalSeconds,
                UserToken = _userResponse,
                RefreshToken = await BuildRefreshToken()
            };
            return user;
        }

        public async Task<string> BuildToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = await GetCurrentKey();
            var user = await GetUser();

            if (_useClaims)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                if (userRoles.Count > 0)
                {
                    _userRoles = new List<TokenRoleDto>();
                    foreach (var roleName in userRoles)
                    {
                        var roleFound = await _roleManager.FindByNameAsync(roleName);
                        if (roleFound != null)
                        {
                            _userRoles.Add(new TokenRoleDto(roleFound.Name, roleFound.UId));
                        }
                    }
                }

                _userData.FullName = user.FullName;
                _userData.Username = user.UserName;

                _userData.User = new UserToken(user.UId, user.FullName, user.Email!, _userRoles);
                _userResponse = new UserToken(user.UId, user.FullName, user.Email!, _userRoles);

                var tokenClaims = new List<Claim>
                {
                    new Claim("exp", ToUnixEpochDate(DateTime.UtcNow.AddSeconds(_settings.Value.Expiration)).ToString(), ClaimValueTypes.Integer),
                    new Claim("data", JsonConvert.SerializeObject(_userData), JsonClaimValueTypes.Json),
                    new Claim("iat", DateTime.UtcNow.ToString(), ClaimValueTypes.Integer),
                };
                _identityClaimsAccess.AddClaims(tokenClaims);
            }

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddSeconds(_settings.Value.Expiration),
                IssuedAt = DateTime.UtcNow,
                Subject = _identityClaimsAccess,
                Audience = _settings.Value.Audience,
                Issuer = _settings.Value.Issuer,
                SigningCredentials = key,
                TokenType = "JWT"
            });
            tokenHandler.SetDefaultTimesOnTokenCreation = false;
            return tokenHandler.WriteToken(token);
        }
        public async Task<string> BuildRefreshToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = await GetCurrentKey();

            if (_useClaims)
            {
                var tokenClaims = new List<Claim>
            {
                new Claim("exp", ToUnixEpochDate(DateTime.UtcNow.AddSeconds(_settings.Value.RefreshTokenExpiration)).ToString(), ClaimValueTypes.Integer),
                new Claim("data", JsonConvert.SerializeObject(_userData), JsonClaimValueTypes.Json),
                new Claim("iat", DateTime.UtcNow.ToString(), ClaimValueTypes.Integer),
            };

                _identityClaimsRefresh.AddClaims(tokenClaims);
            }

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddSeconds(_settings.Value.RefreshTokenExpiration),
                IssuedAt = DateTime.UtcNow,
                Subject = _identityClaimsRefresh,
                Audience = _settings.Value.Audience,
                Issuer = _settings.Value.Issuer,
                SigningCredentials = key,
                TokenType = "JWT"
            });
            tokenHandler.SetDefaultTimesOnTokenCreation = false;
            return tokenHandler.WriteToken(token);
        }

        private async Task<TIdentityUser?> GetUser()
        {
            if (!_useEmail)
                throw new ArgumentNullException("E-mail deve ser providenciado.");

            if (_user is not null)
                return _user;

            if (_useEmail && (_email != null))
                _user = await _userManager.FindByEmailAsync(_email);

            return _user;
        }

        private Task<SigningCredentials> GetCurrentKey()
        {
            return Task.FromResult(new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_settings.Value.SecretKey)), SecurityAlgorithms.HmacSha256Signature));
        }
        
        public async Task<RefreshTokenValidation> ValidateRefreshToken(string refreshToken)
        {
            var handler = new JsonWebTokenHandler();

            var result = await handler.ValidateTokenAsync(refreshToken, new TokenValidationParameters()
            {
                RequireSignedTokens = true,
                ValidIssuer = _settings.Value.Issuer,
                ValidAudience = _settings.Value.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Value.SecretKey)),
            });

            if (!result.IsValid)
                return new RefreshTokenValidation(false, reason: "Refresh Token failed");

            var dataClaim = result.ClaimsIdentity.FindFirst("data");
            string userName = string.Empty;

            if (dataClaim != null)
            {
                using (JsonDocument doc = JsonDocument.Parse(dataClaim.Value))
                {
                    JsonElement root = doc.RootElement;
                    userName = root.GetProperty("unique_name").GetString();
                }
            }

            if (string.IsNullOrEmpty(userName))
                return new RefreshTokenValidation(false, reason: "User not found in token");

            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
                return new RefreshTokenValidation(false, reason: "User not found");

            if (user.LockoutEnabled && user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.Now)
                return new RefreshTokenValidation(false, reason: "User blocked");

            return new RefreshTokenValidation(true, userId: user.Id.ToString(), email: user.Email!);
        }
        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                .TotalSeconds);
    }
}
