using Microsoft.EntityFrameworkCore;
using PAIFGAMES.FCG.Domain.Repositories;
using PAIFGAMES.FCG.Infra.Context;
using PAIFGAMES.FCG.Infra.Repositories;
using System;

namespace PAIFGAMES.FCG.Api.Configurations
{
    public static class DataConfiguration
    {
        public static void AddDataConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            var baseConnectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(baseConnectionString, sql =>
                {
                    sql.MigrationsHistoryTable("__EFMigrationsHistory", "fcg");
                    sql.EnableRetryOnFailure();
                });
            });

            // repositories read only
            services.AddScoped<IGameReadOnlyRepository, GameReadOnlyRepository>();
            services.AddScoped<IUserReadOnlyRepository, UserReadOnlyRepository>();

            // repositories
            services.AddScoped<IGameRepository, GameRepository>();
        }

        public static IApplicationBuilder ApplyMigrations(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.Migrate();
            }
            return app;
        }
    }
}
