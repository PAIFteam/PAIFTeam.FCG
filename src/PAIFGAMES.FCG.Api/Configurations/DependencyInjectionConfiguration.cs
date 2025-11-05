using MediatR;
using PAIFGAMES.FCG.Domain.Mediator.Notifications;
using PAIFGAMES.FCG.Domain.Mediator;
using PAIFGAMES.FCG.Api.Filters;
using PAIFGAMES.FCG.Api.Configurations.ApiKeyConfig;
using PAIFGAMES.FCG.Infra.Data;
using PAIFGAMES.FCG.Domain.Entities;
using PAIFGAMES.FCG.Infra.Auth.Interfaces;
using PAIFGAMES.FCG.Infra.Auth.Jwt;
using Serilog;
using Microsoft.AspNetCore.Authorization;
using PAIFGAMES.FCG.Api.Helpers.Auth.Policies;
using PAIFGAMES.FCG.Domain.Data;
using PAIFGAMES.FCG.Domain.Repositories;
using PAIFGAMES.FCG.Infra.Repositories;
using PAIFGAMES.FCG.Api.Validators;
using FluentValidation.AspNetCore;
using FluentValidation;

namespace PAIFGAMES.FCG.Api.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static void ResolveDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var assemblyMediatrProject = AppDomain.CurrentDomain.Load("PAIFGAMES.FCG.Domain");
            services.AddMediatR(o => 
            {
                o.RegisterServicesFromAssembly(assemblyMediatrProject);
                o.AddPipelineValidator(services, assemblyMediatrProject);
            });

            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            services.AddScoped<IMediatorHandler, MediatorHandler>();

            // api Key attribute
            services.AddSingleton<ApiKeyAuthorizationFilter>();

            // loggers
            services.AddLogging(builder => builder.AddSerilog());
            services.AddScoped<ILogger<GlobalExceptionHandlingFilter>, Logger<GlobalExceptionHandlingFilter>>();
            services.AddScoped<GlobalExceptionHandlingFilter>();

            // authorization
            services.AddScoped<IAuthorizationHandler, RolesAuthorizationHandler>();
            services.AddSingleton<IAuthorizationMiddlewareResultHandler, ForbiddenAuthorizationResponse>();

            // jwt
            services.AddScoped<IJwtBuilder, JwtBuilderInject<IdentityUserCustom, long>>();

            // repositories read only
            services.AddScoped<IUserReadOnlyRepository, UserReadOnlyRepository>();
            services.AddScoped<IGameReadOnlyRepository, GameReadOnlyRepository>();
            services.AddScoped<IGameUserReadOnlyRepository, GameUserReadOnlyRepository>();

            // repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IGameUserRepository, GameUserRepository>();

            // uow
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}