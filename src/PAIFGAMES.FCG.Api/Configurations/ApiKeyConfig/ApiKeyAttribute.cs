using Microsoft.AspNetCore.Mvc;

namespace PAIFGAMES.FCG.Api.Configurations.ApiKeyConfig
{
    public class ApiKeyAttribute : ServiceFilterAttribute
    {
        public ApiKeyAttribute()
            : base(typeof(ApiKeyAuthorizationFilter))
        {
        }
    }
}