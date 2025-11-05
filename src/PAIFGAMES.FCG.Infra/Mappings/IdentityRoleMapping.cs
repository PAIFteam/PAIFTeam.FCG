using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PAIFGAMES.FCG.Domain.Entities;

namespace PAIFGAMES.FCG.Infra.Mappings
{
    public class IdentityRoleMapping : IEntityTypeConfiguration<IdentityRoleCustom>
    {
        public void Configure(EntityTypeBuilder<IdentityRoleCustom> builder)
        {
            builder.HasKey(a => a.Id)
            .HasName("PKtbrole");

            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasColumnType("bigint")
                .ValueGeneratedOnAdd()
                .IsRequired(true);

            builder.Property(a => a.UId)
                .HasColumnName("uid")
                .HasColumnType("uniqueidentifier")
                .IsRequired(true);

            builder
                .Property(a => a.Name)
                .HasColumnName("name")
                .HasColumnType("varchar(50)")
                .IsRequired(true);

            builder
                .Property(a => a.NormalizedName)
                .HasColumnName("name_normalized")
                .HasColumnType("varchar(50)")
                .IsRequired(true);

            builder
                .Property(a => a.ConcurrencyStamp)
                .HasColumnName("concurrency_stamp")
                .HasColumnType("varchar(40)")
                .IsRequired(true);

            builder
                .Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("datetime2")
                .IsRequired(true);

            builder
                .Property(a => a.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("datetime2")
                .IsRequired(true);

            builder
                .ToTable("tb_role", "fcg");
        }
    }
}
