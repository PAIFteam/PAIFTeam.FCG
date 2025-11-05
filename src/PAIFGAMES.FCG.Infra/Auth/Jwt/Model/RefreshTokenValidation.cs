namespace PAIFGAMES.FCG.Infra.Auth.Jwt.Model
{
    public class RefreshTokenValidation
    {
        public bool IsValid { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Reason { get; }

        public static implicit operator bool(RefreshTokenValidation validation) => validation.IsValid;

        public RefreshTokenValidation(bool isValid, string reason = null, string userId = null, string email = null)
        {
            IsValid = isValid;
            Reason = reason;
            UserId = userId;
            Email = email;
        }
    }
}
