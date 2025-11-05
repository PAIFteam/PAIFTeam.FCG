using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PAIFGAMES.FCG.Api.Helpers.Auth.Policies;
using PAIFGAMES.FCG.Api.Helpers.Auth.Schemes;
using PAIFGAMES.FCG.Domain.Entities;
using PAIFGAMES.FCG.Infra.Context;
using System.Text;

namespace PAIFGAMES.FCG.Api.Configurations
{
    public static class AuthConfiguration
    {
        public static void AddAuthConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));


            services.AddIdentity<IdentityUserCustom, IdentityRoleCustom>(options =>
            {
                options.Lockout.AllowedForNewUsers = false;

                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 1;
                options.Password.RequiredUniqueChars = 0;
            })
            .AddRoleManager<RoleManager<IdentityRoleCustom>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {

                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
        .AddJwtBearer("Identity", options =>
        {
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:SecretKey"]!);
            options.Audience = configuration["Jwt:Audience"];

            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),

                ValidateIssuer = true,
                ValidIssuer = configuration["Jwt:Issuer"],

                ValidateAudience = true,
                ValidAudience = configuration["Jwt:Audience"],
                ValidAudiences = new string[] { configuration["Jwt:Audience"] },

                ValidateLifetime = true
            };
        });

            services.AddAuthentication()
             .AddApiKeySupport(options => { });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                options.AddPolicy("Role[Admin]", policy => policy.Requirements.Add(new RolesRequirement("Admin")));
            });
        }
    }
}
