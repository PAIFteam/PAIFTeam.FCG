using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace PAIFGAMES.FCG.Infra.Mappings
{
    public class IdentityUserLoginMapping : IEntityTypeConfiguration<IdentityUserLogin<long>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserLogin<long>> builder)
        {
            builder
                .HasKey(a => new { a.LoginProvider, a.ProviderKey })
                .HasName("PKtbuserlogin");

            builder
                .Property(a => a.LoginProvider)
                .HasColumnName("login_provider")
                .HasColumnType("varchar(40)")
                .IsRequired(true);

            builder
                .Property(a => a.ProviderKey)
                .HasColumnName("provider_key")
                .HasColumnType("varchar(40)")
                .IsRequired(true);

            builder
                .Property(a => a.ProviderDisplayName)
                .HasColumnName("provider_display_name")
                .HasColumnType("varchar(40)")
                .IsRequired(false);

            builder
                .Property(a => a.UserId)
                .HasColumnName("user_id")
                .HasColumnType("bigint")
                .IsRequired(true);

            builder.ToTable("tb_user_login", "fcg");
        }
    }
}
