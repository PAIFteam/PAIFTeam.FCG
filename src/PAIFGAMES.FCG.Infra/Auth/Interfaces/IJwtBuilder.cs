using PAIFGAMES.FCG.Infra.Auth.Jwt.Model;

namespace PAIFGAMES.FCG.Infra.Auth.Interfaces
{
    public interface IJwtBuilder
    {
        IJwtBuilder WithEmail(string email);
        IJwtBuilder WithUserClaims();
        Task<AuthResponse> BuildUserResponse(CancellationToken cancellationToken);
        Task<RefreshTokenValidation> ValidateRefreshToken(string refreshToken);
    }
}
