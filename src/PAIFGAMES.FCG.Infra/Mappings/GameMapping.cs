using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PAIFGAMES.FCG.Domain.Entities;

namespace PAIFGAMES.FCG.Infra.Mappings
{
    public class GameMapping : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.HasKey(a => a.Id)
            .HasName("PKtbgame");

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
                .HasColumnType("varchar(100)")
                .IsRequired(true);

            builder
                .Property(a => a.Value)
                .HasColumnName("value")
                .HasColumnType("decimal(18,2)")
                .IsRequired(true);

            builder.ToTable("tb_game", "fcg");
        }
    }
}
