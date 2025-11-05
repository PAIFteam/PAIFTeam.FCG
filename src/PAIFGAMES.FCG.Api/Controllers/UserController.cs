using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PAIFGAMES.FCG.Domain.Entities;
using PAIFGAMES.FCG.Domain.Mediator;
using PAIFGAMES.FCG.Domain.Mediator.Notifications;
using PAIFGAMES.FCG.Infra.Auth.Interfaces;
using PAIFGAMES.FCG.Api.Models;
using PAIFGAMES.FCG.Domain.Users.Commands;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using PAIFGAMES.FCG.Api.Configurations.ApiKeyConfig;
using Newtonsoft.Json;
using PAIFGAMES.FCG.Domain.Extensions;
using PAIFGAMES.FCG.Domain.Users.Filter;
using PAIFGAMES.FCG.Domain.Users.Queries;

namespace PAIFGAMES.FCG.Api.Controllers
{
    [Route("api")]
    public class UserController : ApiController
    {
        private readonly IMediatorHandler _mediator;
        private readonly UserManager<IdentityUserCustom> _userManager;
        private readonly SignInManager<IdentityUserCustom> _signInManager;
        private readonly ILogger<UserController> _logger;
        private readonly IJwtBuilder _jwtBuilder;

        public UserController(ILogger<UserController> logger, INotificationHandler<DomainNotification> notifications, UserManager<IdentityUserCustom> userManager, SignInManager<IdentityUserCustom> signInManager, IMediatorHandler mediator, IJwtBuilder jwtBuilder) : base(logger, notifications)
        {
            _mediator = mediator;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _jwtBuilder = jwtBuilder;
        }

        [Authorize(AuthenticationSchemes = "ApiKey", Policy = "Role[Admin]")]
        [HttpPost("user/register")]
        public async Task<IActionResult> CreateUserAsync([FromBody] RegisterUserCommand command, CancellationToken cancellationToken)
        {
            var register = await _mediator.SendCommand(command, cancellationToken);
            return ResponseApi(register);
        }

        [HttpPost("user/login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model, CancellationToken cancellationToken)
        {
            try
            {
                var signInResult = await _signInManager.PasswordSignInAsync(
                      userName: model.Email!,
                      password: model.Password!,
                      isPersistent: false,
                      lockoutOnFailure: false
                      );

                if (signInResult.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    var userRoles = await _userManager.GetRolesAsync(user!);
                    var token = await _jwtBuilder
                        .WithUserClaims()
                        .WithEmail(model.Email)
                        .BuildUserResponse(cancellationToken);

                    return ResponseApi(token);
                }
                else if (signInResult.IsLockedOut)
                {
                    await _notifications.Handle(new DomainNotification("User", "Usuário bloqueado por tentativas inválidas."), cancellationToken);
                }
                else if (signInResult.IsNotAllowed)
                {
                    await _notifications.Handle(new DomainNotification("User", "Usuário não permitido para login."), cancellationToken);
                }
                else
                {
                    await _notifications.Handle(new DomainNotification("User", "Usuário ou senha incorretos."), cancellationToken);
                }
            }
            catch (Exception)
            {
                return ResponseApi();
            }
            return ResponseApi();
        }

        [HttpPost("user/refreshtoken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel model, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(model.RefreshToken))
            {
                await _notifications.Handle(new DomainNotification("RefreshToken", "O campo RefreshToken é obrigatório"), cancellationToken);
                return ResponseApi();
            }

            var validation = await _jwtBuilder.ValidateRefreshToken(model.RefreshToken);
            if (validation.IsValid)
            {
                var token = await _jwtBuilder
                        .WithUserClaims()
                        .WithEmail(validation.Email)
                        .BuildUserResponse(cancellationToken);

                return ResponseApi(token);
            }
            return ResponseApi(validation);
        }

        [Authorize(AuthenticationSchemes = "Identity", Policy = "Role[Admin]")]
        [HttpGet("user/{userUId}")]
        public async Task<IActionResult> GetByUId(Guid userUId, CancellationToken cancellationToken)
        {
            var user = await _mediator.Query(new GetUserByUIdQuery(userUId), cancellationToken);
            return ResponseApi(user);
        }

        [Authorize(AuthenticationSchemes = "Identity", Policy = "Role[Admin]")]
        [HttpGet("users/all")]
        public async Task<IActionResult> GetAll([FromQuery] UserFilterModel userFilter, [FromQuery] PageFilterModel filtroPaginacao, CancellationToken cancellationToken)
        {
            var users = await _mediator.Query(new GetAllUsersQuery(userFilter, filtroPaginacao), cancellationToken);

            var metadata = new
            {
                users.TotalCount,
                users.PageSize,
                users.CurrentPage,
                users.TotalPages,
                users.HasPrevious,
                users.HasNext
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");
            return ResponseApi(users);
        }

        [Authorize(AuthenticationSchemes = "Identity", Policy = "Role[Admin]")]
        [HttpPut("user/attach-role")]
        public async Task<IActionResult> AttachRole([FromBody] AttachRoleUserCommand command, CancellationToken cancellationToken)
        {
            await _mediator.SendCommand(command, cancellationToken);
            return ResponseApi();
        }

        [Authorize(AuthenticationSchemes = "Identity", Policy = "Role[Admin]")]
        [HttpPut("user/detach-role")]
        public async Task<IActionResult> DetachRole([FromBody] DetachRoleUserCommand command, CancellationToken cancellationToken)
        {
            await _mediator.SendCommand(command, cancellationToken);
            return ResponseApi();
        }

        [Authorize(AuthenticationSchemes = "Identity", Policy = "Role[Admin]")]
        [HttpPut("user/attach-game")]
        public async Task<IActionResult> AttachGame([FromBody] AttachGameUserCommand command, CancellationToken cancellationToken)
        {
            await _mediator.SendCommand(command, cancellationToken);
            return ResponseApi();
        }

        [Authorize(AuthenticationSchemes = "Identity", Policy = "Role[Admin]")]
        [HttpPut("user/detach-game")]
        public async Task<IActionResult> DetachGame([FromBody] DetachGameUserCommand command, CancellationToken cancellationToken)
        {
            await _mediator.SendCommand(command, cancellationToken);
            return ResponseApi();
        }

    }
}
