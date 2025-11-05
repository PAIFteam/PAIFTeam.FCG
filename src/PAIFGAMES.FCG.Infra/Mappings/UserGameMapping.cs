using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PAIFGAMES.FCG.Domain.Entities;

namespace PAIFGAMES.FCG.Infra.Mappings
{
    public class UserGameMapping : IEntityTypeConfiguration<UserGame>
    {
        public void Configure(EntityTypeBuilder<UserGame> builder)
        {
            builder
                .HasKey(ug => new { ug.UserId, ug.GameId })
                .HasName("PKtbusergame");

            builder
                .Property(ug => ug.UserId)
                .HasColumnName("user_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder
                .Property(ug => ug.GameId)
                .HasColumnName("game_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder
                .HasOne(ug => ug.User)
                .WithMany(u => u.UserGames)
                .HasForeignKey(ug => ug.UserId);

            builder
                .HasOne(ug => ug.Game)
                .WithMany(g => g.UserGames)
                .HasForeignKey(ug => ug.GameId);

            builder
                .ToTable("tb_user_game", "fcg");
        }
    }
}
