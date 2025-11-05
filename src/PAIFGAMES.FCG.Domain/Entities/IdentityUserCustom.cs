using Microsoft.AspNetCore.Identity;

namespace PAIFGAMES.FCG.Domain.Entities
{
    public class IdentityUserCustom : IdentityUser<long>
    {
        public IdentityUserCustom() { }
        public IdentityUserCustom(string fullName, string email)
        {
            UId = Guid.NewGuid();
            FullName = fullName;
            Email = email;
            UserName = email;
        }

        public IdentityUserCustom(long id,
                    Guid uid,
                    string fullName,
                    DateTime createdAt,
                    DateTime updatedAt,
                    int accessFailedCount,
                    string? concurrencyStamp,
                    string? email,
                    bool emailConfirmed,
                    string? userName,
                    bool lockoutEnabled,
                    DateTimeOffset? lockoutEnd,
                    string? normalizedEmail,
                    string? normalizedUserName,
                    string? passwordHash,
                    string? phoneNumber,
                    bool phoneNumberConfirmed,
                    string? securityStamp,
                    bool twoFactorEnable
                   )
        {
            Id = id;
            UId = uid;
            FullName = fullName;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            AccessFailedCount = accessFailedCount;
            ConcurrencyStamp = concurrencyStamp;
            Email = email;
            EmailConfirmed = emailConfirmed;
            UserName = userName;
            LockoutEnabled = lockoutEnabled;
            LockoutEnd = lockoutEnd;
            NormalizedEmail = normalizedEmail;
            NormalizedUserName = normalizedUserName;
            PasswordHash = passwordHash;
            PhoneNumber = phoneNumber;
            PhoneNumberConfirmed = phoneNumberConfirmed;
            SecurityStamp = securityStamp;
            TwoFactorEnabled = twoFactorEnable;
        }

        public Guid UId { get; private set; }
        public string FullName { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        public DateTime UpdatedAt { get; private set; } = DateTime.Now;
        public virtual ICollection<UserGame> UserGames { get; set; }

        public IdentityUserCustom UpdateAt()
        {
            UpdatedAt = DateTime.Now;

            return this;
        }
        public static class IdentityUserCustomFactory
        {
            public static IdentityUserCustom Update(long id,
                Guid uid,
                string fullName,
                DateTime createdAt,
                DateTime updatedAt,
                int accessFailedCount,
                string? concurrencyStamp,
                string? email,
                bool emailConfirmed,
                string? userName,
                bool lockoutEnabled,
                DateTimeOffset? lockoutEnd,
                string? normalizedEmail,
                string? normalizedUserName,
                string? passwordHash,
                string? phoneNumber,
                bool phoneNumberConfirmed,
                string? securityStamp,
                bool twoFactorEnable
               )
            {
                return new()
                {
                    Id = id,
                    UId = uid,
                    FullName = fullName,
                    CreatedAt = createdAt,
                    UpdatedAt = updatedAt,
                    AccessFailedCount = accessFailedCount,
                    ConcurrencyStamp = concurrencyStamp,
                    Email = email,
                    EmailConfirmed = emailConfirmed,
                    UserName = userName,
                    LockoutEnabled = lockoutEnabled,
                    LockoutEnd = lockoutEnd,
                    NormalizedEmail = normalizedEmail,
                    NormalizedUserName = normalizedUserName,
                    PasswordHash = passwordHash,
                    PhoneNumber = phoneNumber,
                    PhoneNumberConfirmed = phoneNumberConfirmed,
                    SecurityStamp = securityStamp,
                    TwoFactorEnabled = twoFactorEnable
                };
            }
        }
    }
}
