using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace PAIFGAMES.FCG.Infra.Mappings
{
    public class IdentityUserRoleMapping : IEntityTypeConfiguration<IdentityUserRole<long>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<long>> builder)
        {
            builder
                .HasKey(a => new { a.UserId, a.RoleId })
                .HasName("PKtbuserrole");
            builder
                .Property(a => a.UserId)
                .HasColumnName("user_id")
                .HasColumnType("bigint")
                .IsRequired(true);

            builder
                .Property(a => a.RoleId)
                .HasColumnName("role_id")
                .HasColumnType("bigint")
                .IsRequired(true);

            builder.ToTable("tb_user_role", "fcg");
        }
    }
}
