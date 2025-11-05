using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace PAIFGAMES.FCG.Infra.Mappings
{
    public class IdentityRoleClaimMapping : IEntityTypeConfiguration<IdentityRoleClaim<long>>
    {
        public void Configure(EntityTypeBuilder<IdentityRoleClaim<long>> builder)
        {
            builder.HasKey(a => a.Id)
            .HasName("PKtbroleclaim");

            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasColumnType("int")
                .ValueGeneratedOnAdd()
                .IsRequired(true);

            builder
                .Property(a => a.RoleId)
                .HasColumnName("role_id")
                .HasColumnType("bigint")
                .IsRequired(true);

            builder
                .Property(a => a.ClaimType)
                .HasColumnName("claim_type")
                .HasColumnType("varchar(50)")
                .IsRequired(true);

            builder
                .Property(a => a.ClaimValue)
                .HasColumnName("claim_value")
                .HasColumnType("varchar(50)")
                .IsRequired(true);

            builder.ToTable("tb_role_claim", "fcg");
        }
    }
}
