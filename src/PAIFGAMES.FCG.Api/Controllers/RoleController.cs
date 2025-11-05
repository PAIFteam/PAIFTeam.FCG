using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAIFGAMES.FCG.Domain.Mediator.Notifications;
using PAIFGAMES.FCG.Domain.Mediator;
using PAIFGAMES.FCG.Domain.Roles.Commands;
using PAIFGAMES.FCG.Domain.Roles.Queries;

namespace PAIFGAMES.FCG.Api.Controllers
{
    [Route("api")]
    public class RoleController : ApiController
    {
        private readonly IMediatorHandler _mediator;
        public RoleController(ILogger<RoleController> logger, INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator) : base(logger, notifications)
        {
            _mediator = mediator;
        }

        [Authorize(AuthenticationSchemes = "Identity", Policy = "Role[Admin]")]
        [HttpPost("role/register")]
        public async Task<IActionResult> Register([FromBody] RegisterRoleCommand command, CancellationToken cancellationToken)
        {
            await _mediator.SendCommand(command, cancellationToken);
            return ResponseApi();
        }

        [Authorize(AuthenticationSchemes = "Identity", Policy = "Role[Admin]")]
        [HttpGet("roles/all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var roles = await _mediator.Query(new GetAllRolesQuery(), cancellationToken);
            return ResponseApi(roles);
        }

        [Authorize(AuthenticationSchemes = "Identity", Policy = "Role[Admin]")]
        [HttpGet("role/{roleUId}")]
        public async Task<IActionResult> GetByUId(Guid roleUId, CancellationToken cancellationToken)
        {
            var role = await _mediator.Query(new GetRoleByUIdQuery(roleUId), cancellationToken);
            return ResponseApi(role);
        }

        [Authorize(AuthenticationSchemes = "Identity", Policy = "Role[Admin]")]
        [HttpPut("role/update")]
        public async Task<IActionResult> Update([FromBody] UpdateRoleCommand command, CancellationToken cancellationToken)
        {
            await _mediator.SendCommand(command, cancellationToken);
            return ResponseApi();
        }

        [Authorize(AuthenticationSchemes = "Identity", Policy = "Role[Admin]")]
        [HttpDelete("role/{roleUId}")]
        public async Task<IActionResult> Delete(Guid roleUId, CancellationToken cancellationToken)
        {
            await _mediator.SendCommand(new DeleteRoleCommand(roleUId), cancellationToken);
            return ResponseApi();
        } 
    }
}