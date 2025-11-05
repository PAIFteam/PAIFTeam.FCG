using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PAIFGAMES.FCG.Domain.Entities;

namespace PAIFGAMES.FCG.Infra.Mappings
{
    public class IdentityUserMapping : IEntityTypeConfiguration<IdentityUserCustom>
    {
        public void Configure(EntityTypeBuilder<IdentityUserCustom> builder)
        {
            builder.HasKey(a => a.Id)
                .HasName("PKtbuser");

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
                .Property(a => a.FullName)
                .HasColumnName("fullname")
                .HasColumnType("varchar(50)")
                .IsRequired(true);

            builder
                .Property(a => a.UserName)
                .HasColumnName("username")
                .HasColumnType("varchar(50)")
                .IsRequired(true);

            builder
                .Property(a => a.NormalizedUserName)
                .HasColumnName("normalized_username")
                .HasColumnType("varchar(50)")
                .IsRequired(true);

            builder
                .Property(a => a.Email)
                .HasColumnName("email")
                .HasColumnType("varchar(70)")
                .IsRequired(true);

            builder
                .Property(a => a.NormalizedEmail)
                .HasColumnName("normalized_email")
                .HasColumnType("varchar(70)")
                .IsRequired(true);

            builder
                .Property(a => a.EmailConfirmed)
                .HasColumnName("is_email_confirmed")
                .HasColumnType("bit")
                .IsRequired(true);

            builder
                .Property(a => a.PasswordHash)
                .HasColumnName("password_hash")
                .HasColumnType("varchar(500)")
                .IsRequired(true);

            builder
                .Property(a => a.SecurityStamp)
                .HasColumnName("uid_security_stamp")
                .HasColumnType("varchar(40)")
                .IsRequired(true);

            builder
                .Property(a => a.ConcurrencyStamp)
                .HasColumnName("uid_concurrency_stamp")
                .HasColumnType("varchar(40)")
                .IsRequired(true);

            builder
                .Property(a => a.PhoneNumber)
                .HasColumnName("phone_number")
                .HasColumnType("varchar(25)")
                .IsRequired(false);

            builder
                .Property(a => a.PhoneNumberConfirmed)
                .HasColumnName("is_phone_number_confirmed")
                .HasColumnType("bit")
                .IsRequired(true);

            builder
                .Property(a => a.TwoFactorEnabled)
                .HasColumnName("is_two_factor_enabled")
                .HasColumnType("bit")
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
                .Property(a => a.LockoutEnd)
                .HasColumnName("lockout_end_time")
                .HasColumnType("datetimeoffset")
                .IsRequired(false);

            builder
                .Property(a => a.LockoutEnabled)
                .HasColumnName("is_lockout_enabled")
                .HasColumnType("bit")
                .IsRequired(true);

            builder
                .Property(a => a.AccessFailedCount)
                .HasColumnName("num_access_failed_count")
                .HasColumnType("smallint")
                .IsRequired(true);

            builder
                .ToTable("tb_user", "fcg");
        }
    }
}
