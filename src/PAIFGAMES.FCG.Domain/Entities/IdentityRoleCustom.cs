using Microsoft.AspNetCore.Identity;

namespace PAIFGAMES.FCG.Domain.Entities
{
    public class IdentityRoleCustom : IdentityRole<long>
    {
        public IdentityRoleCustom(string name)
        {
            Name = name;
            UId = Guid.NewGuid();
            ConcurrencyStamp = Guid.NewGuid().ToString();
        }
        public Guid UId { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        public DateTime UpdatedAt { get; private set; } = DateTime.Now;

        public IdentityRoleCustom UpdateAt()
        {
            UpdatedAt = DateTime.Now;

            return this;
        }
    }
}
