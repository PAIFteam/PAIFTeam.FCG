using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace PAIFGAMES.FCG.Infra.Mappings
{
    public class IdentityUserClaimMapping : IEntityTypeConfiguration<IdentityUserClaim<long>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserClaim<long>> builder)
        {
            builder.HasKey(a => a.Id)
                .HasName("PKtbuserclaim");

            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasColumnType("bigint")
                .ValueGeneratedOnAdd()
                .IsRequired(true);

            builder
                .Property(a => a.UserId)
                .HasColumnName("user_id")
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

            builder.ToTable("tb_user_claim", "fcg");
        }
    }
}
