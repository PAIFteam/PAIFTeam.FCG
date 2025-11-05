using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PAIFGAMES.FCG.Domain.Entities;
using PAIFGAMES.FCG.Infra.Mappings;

namespace PAIFGAMES.FCG.Infra.Context
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUserCustom, IdentityRoleCustom, long>
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }
        public DbSet<Game> Game { get; set; }
        public DbSet<UserGame> UserGames { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new GameMapping());
            builder.ApplyConfiguration(new UserGameMapping());
            builder.ApplyConfiguration(new IdentityUserMapping());
            builder.ApplyConfiguration(new IdentityRoleMapping());
            builder.ApplyConfiguration(new IdentityUserRoleMapping());
            builder.ApplyConfiguration(new IdentityUserClaimMapping());
            builder.ApplyConfiguration(new IdentityUserLoginMapping());
            builder.ApplyConfiguration(new IdentityRoleClaimMapping());
            builder.ApplyConfiguration(new IdentityUserTokenMapping());
            builder.HasDefaultSchema("fcg");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                base.OnConfiguring(optionsBuilder);

                optionsBuilder
                    .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                    .UseSqlServer(_configuration.GetConnectionString("DefaultConnection"), sql => 
                    {
                        sql.MigrationsHistoryTable("__EFMigrationsHistory", "fcg");
                        sql.EnableRetryOnFailure();
                    });
            }
        }
    }
}