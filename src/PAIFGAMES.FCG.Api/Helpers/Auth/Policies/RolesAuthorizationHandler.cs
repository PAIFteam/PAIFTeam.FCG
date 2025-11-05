using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using PAIFGAMES.FCG.Infra.Auth.Users;

namespace PAIFGAMES.FCG.Api.Helpers.Auth.Policies
{
    public class RolesAuthorizationHandler : AuthorizationHandler<RolesRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RolesRequirement requirement)
        {
            var userDataClaim = context?.User.FindFirst("data");

            if (context?.User?.Identity?.AuthenticationType == "ApiKey")
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            if (userDataClaim == null)
            {
                return Task.CompletedTask;
            }

            var userData = JsonConvert.DeserializeObject<UserData>(userDataClaim.Value);

            if (userData != null && userData.User != null && userData.User.Roles != null &&
        requirement.Roles.All(role => userData.User.Roles.Any(userRole => userRole.RoleName == role)))
            {
                context?.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public class RolesRequirement : IAuthorizationRequirement
    {
        public string[] Roles { get; }

        public RolesRequirement(params string[] roles)
        {
            Roles = roles;
        }
    }
}
