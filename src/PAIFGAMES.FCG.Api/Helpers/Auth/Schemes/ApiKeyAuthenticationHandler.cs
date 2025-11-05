using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using PAIFGAMES.FCG.Api.Response;
using System.Net.Mime;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace PAIFGAMES.FCG.Api.Helpers.Auth.Schemes
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private enum AuthenticationFailureReason
        {
            NONE = 0,
            API_KEY_HEADER_NOT_PROVIDED,
            API_KEY_HEADER_VALUE_NULL,
            API_KEY_INVALID
        }

        private readonly ILogger<ApiKeyAuthenticationHandler> _logger;
        private readonly IConfiguration _configuration;

        private AuthenticationFailureReason _failureReason = AuthenticationFailureReason.NONE;

        public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationOptions> options,
                                           ILoggerFactory loggerFactory,
                                           ILogger<ApiKeyAuthenticationHandler> logger,
                                           UrlEncoder encoder,
                                           IConfiguration configuration) : base(options, loggerFactory, encoder)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!TryGetApiKeyHeader(out string providedApiKey, out AuthenticateResult authenticateResult))
            {
                return authenticateResult;
            }

            if (await ApiKeyCheckAsync(providedApiKey))
            {
                var identity = new ClaimsIdentity(ApiKeyAuthenticationOptions.Scheme);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(), ApiKeyAuthenticationOptions.Scheme);
                return AuthenticateResult.Success(ticket);
            }

            _failureReason = AuthenticationFailureReason.API_KEY_INVALID;
            return AuthenticateResult.NoResult();
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.Headers.Append(HeaderNames.WWWAuthenticate, $@"Authorization realm=""{ApiKeyAuthenticationOptions.DefaultScheme}""");
            Response.StatusCode = StatusCodes.Status401Unauthorized;
            Response.ContentType = MediaTypeNames.Application.Json;

            var result = new Result
            {
                Succeeded = false,
                Errors = _failureReason switch
                {
                    AuthenticationFailureReason.API_KEY_HEADER_NOT_PROVIDED => new[] { new MessageError() { Key = "ApiKeyAuthentication", Message = "JWT Token ou API Key devem ser fornecidos." } },
                    AuthenticationFailureReason.API_KEY_HEADER_VALUE_NULL => new[] { new MessageError() { Key = "ApiKeyAuthentication", Message = "API Key vazia." } },
                    AuthenticationFailureReason.NONE or AuthenticationFailureReason.API_KEY_INVALID or _ => new[] { new MessageError() { Key = "ApiKeyAuthentication", Message = "API Key inválida." } }
                }
            };

            using var responseStream = new MemoryStream();
            await JsonSerializer.SerializeAsync(responseStream, result);
            await Response.BodyWriter.WriteAsync(responseStream.ToArray());
        }

        #region Privates
        private bool TryGetApiKeyHeader(out string apiKeyHeaderValue, out AuthenticateResult result)
        {
            apiKeyHeaderValue = null;
            if (!Request.Headers.TryGetValue("X-Api-Key", out var apiKeyHeaderValues))
            {
                _logger.LogError("JWT Token ou API Key não providenciados.");

                _failureReason = AuthenticationFailureReason.API_KEY_HEADER_NOT_PROVIDED;
                result = AuthenticateResult.Fail("JWT Token ou API Key não providenciados.");

                return false;
            }

            apiKeyHeaderValue = apiKeyHeaderValues.FirstOrDefault();
            if (apiKeyHeaderValues.Count == 0 || string.IsNullOrWhiteSpace(apiKeyHeaderValue))
            {
                _logger.LogError("API Key vazia.");

                _failureReason = AuthenticationFailureReason.API_KEY_HEADER_VALUE_NULL;
                result = AuthenticateResult.Fail("API Key vazia.");

                return false;
            }

            result = null;
            return true;
        }

        private Task<bool> ApiKeyCheckAsync(string apiKey)
        {
            if (apiKey != _configuration["AdminApiKey"])
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }
        #endregion
    }

    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "ApiKey";

        public static string Scheme => DefaultScheme;
        public static string AuthenticationType => DefaultScheme;
    }

    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddApiKeySupport(this AuthenticationBuilder authenticationBuilder, Action<ApiKeyAuthenticationOptions> options)
            => authenticationBuilder.AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationOptions.DefaultScheme, options);
    }
}
