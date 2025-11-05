using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAIFGAMES.FCG.Domain.Mediator.Notifications;
using PAIFGAMES.FCG.Domain.Mediator;
using PAIFGAMES.FCG.Domain.Games.Queries;
using PAIFGAMES.FCG.Domain.Games.Commands;

namespace PAIFGAMES.FCG.Api.Controllers
{
    [Route("api")]
    public class GameController : ApiController
    {
        private readonly IMediatorHandler _mediator;

        public GameController(ILogger<GameController> logger, INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator) : base(logger, notifications)
        {
            _mediator = mediator;
        }

        [Authorize(AuthenticationSchemes = "Identity", Policy = "Role[Admin]")]
        [HttpPost("game/register")]
        public async Task<IActionResult> Register([FromBody] RegisterGameCommand command, CancellationToken cancellationToken)
        {
            await _mediator.SendCommand(command, cancellationToken);
            return ResponseApi();
        }

        [Authorize(AuthenticationSchemes = "Identity", Policy = "Role[Admin]")]
        [HttpGet("game/{gameUId}")]
        public async Task<IActionResult> GetByUId(Guid gameUId, CancellationToken cancellationToken)
        {
            var game = await _mediator.Query(new GetGameByUIdQuery(gameUId), cancellationToken);
            return ResponseApi(game);
        }

        [Authorize(AuthenticationSchemes = "Identity")]
        [HttpGet("games/all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var games = await _mediator.Query(new GetAllGamesQuery(), cancellationToken);
            return ResponseApi(games);
        }

        [Authorize(AuthenticationSchemes = "Identity", Policy = "Role[Admin]")]
        [HttpPut("game/update")]
        public async Task<IActionResult> Update([FromBody] UpdateGameCommand command, CancellationToken cancellationToken)
        {
            await _mediator.SendCommand(command, cancellationToken);
            return ResponseApi();
        }

        [Authorize(AuthenticationSchemes = "Identity", Policy = "Role[Admin]")]
        [HttpDelete("game/{gameUId}")]
        public async Task<IActionResult> Delete(Guid gameUId, CancellationToken cancellationToken)
        {
            await _mediator.SendCommand(new DeleteGameCommand(gameUId), cancellationToken);
            return ResponseApi();
        }

    }
}