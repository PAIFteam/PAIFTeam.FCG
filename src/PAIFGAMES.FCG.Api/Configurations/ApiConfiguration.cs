using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using PAIFGAMES.FCG.Api.Filters;
using PAIFGAMES.FCG.Infra.Auth.Jwt;
using Asp.Versioning;

namespace PAIFGAMES.FCG.Api.Configurations
{
    public static class ApiConfiguration
    {
        public static void AddApiConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureOptions<JwtSettingsSetup>();

            services
                .AddMvcCore(o =>
                {
                    o.OutputFormatters.Remove(new XmlDataContractSerializerOutputFormatter());
                    o.Filters.Add(new ServiceFilterAttribute(typeof(GlobalExceptionHandlingFilter)));
                })
                .AddNewtonsoftJson(o =>
                {
                    o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    o.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                })
                .ConfigureApiBehaviorOptions(a => a.SuppressModelStateInvalidFilter = true)
                .AddApiExplorer()
                .AddCors(options =>
                {
                    options.AddPolicy("All",
                        builder =>
                            builder
                                .AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader());
                })
                .AddFormatterMappings();

            services.AddControllers(options =>
            {
                options.AllowEmptyInputInBodyModelBinding = true;
                options.Filters.Add(new ServiceFilterAttribute(typeof(GlobalExceptionHandlingFilter)));
                options.Filters.Add(typeof(ValidateCommandFilter));
            });
        }
        public static void UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}