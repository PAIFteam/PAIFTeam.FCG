using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;

namespace PAIFGAMES.FCG.Infra.Mappings
{
    public class IdentityUserTokenMapping : IEntityTypeConfiguration<IdentityUserToken<long>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserToken<long>> builder)
        {
            builder
                .HasKey(a => new { a.UserId, a.LoginProvider, a.Name })
                .HasName("PKtbusertoken");

            builder
                .Property(a => a.UserId)
                .HasColumnName("user_id")
                .HasColumnType("bigint")
                .IsRequired(true);

            builder
                .Property(a => a.LoginProvider)
                .HasColumnName("login_provider")
                .HasColumnType("varchar(50)")
                .IsRequired(true);

            builder
                .Property(a => a.Name)
                .HasColumnName("name")
                .HasColumnType("varchar(50)")
                .IsRequired(true);

            builder
                .Property(a => a.Value)
                .HasColumnName("value")
                .HasColumnType("varchar(250)")
                .IsRequired(false);

            builder.ToTable("tb_user_token", "fcg");
        }
    }
}
