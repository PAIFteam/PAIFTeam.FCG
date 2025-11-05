using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Authorization;
using PAIFGAMES.FCG.Api.Response;

namespace PAIFGAMES.FCG.Api.Helpers.Auth.Policies
{
    public class ForbiddenAuthorizationResponse : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();

        public async Task HandleAsync(
            RequestDelegate next,
            HttpContext context,
            AuthorizationPolicy policy,
            PolicyAuthorizationResult authorizeResult)
        {
            if (authorizeResult.Forbidden
                && authorizeResult.AuthorizationFailure!.FailedRequirements.OfType<RolesRequirement>().Any())
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json; charset=utf-8";

                var response = new Result()
                {
                    Succeeded = false,
                    Errors = new[] { new MessageError() { Key = "Forbidden", Message = "Você não tem perfil para acessar este recurso." } }
                };

                await context.Response.WriteAsJsonAsync(response);
                return;
            }

            await defaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }
    }
}
