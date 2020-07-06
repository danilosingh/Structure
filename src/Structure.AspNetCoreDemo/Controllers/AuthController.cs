

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Structure.AspNetCore;
using Structure.Identity;
using Structure.MultiTenancy;
using Structure.Security.Authorization;
using Structure.Started.AspNetCore.Authorization;
using Structure.Tests.Shared.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Structure.AspNetCoreDemo.Controllers
{

    public class AuthController : ApiControllerBase
    {
        private readonly JwtBearerTokenAuthenticationService<User, IdentityToken, RefreshableJwtAuthenticationResult<User>> authenticationService;
        private readonly ICurrentTenant currentTenant;

        public AuthController(JwtBearerTokenAuthenticationService<User, IdentityToken, RefreshableJwtAuthenticationResult<User>> authenticationService, ICurrentTenant currentTenant)
        {
            this.authenticationService = authenticationService;
            this.currentTenant = currentTenant;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate(AuthModel model)
        {
            var result = await authenticationService.AuthenticateAsync(model.Email, model.Password, currentTenant.Id);
            return CreateExternalResult(result);
        }

        public async Task<IActionResult> Update(AuthTenantChangeDto authTenantDto)
        {
            var result = await authenticationService.ChangeAsync(authTenantDto.TenantId, new Claim("Other", authTenantDto.OtherValue));
            return CreateExternalResult(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody]AuthRefreshTokenDto refreshToken)
        {
            var result = await authenticationService.RefreshTokenAsync(refreshToken.RefreshToken);
            return CreateExternalResult(result);
        }

        private IActionResult CreateExternalResult(RefreshableJwtAuthenticationResult<User> result)
        {
            if (result.Type != AuthenticationResultType.Success)
            {
                throw new AuthorizationException(GetResultMessage(result));
            }

            return Ok(new ExternalJwtAuthenticateResult()
            {
                TenantId = result.TenantId,
                AccessToken = result.AccessToken,
                RefreshToken = result.RefreshToken,
                ExpireInSeconds = result.ExpireInSeconds,
                User = CreateExternalUser(result.User)
            });
        }

        private string GetResultMessage(JwtAuthenticationResult<User> result)
        {
            switch (result.Type)
            {
                case AuthenticationResultType.InvalidUserNameOrEmailAddress:
                    return "Usuário inválido";
                case AuthenticationResultType.InvalidPassword:
                    return "Usuário ou senha inválidos";
                case AuthenticationResultType.UserIsNotActive:
                    return "Usuário inativo";
                case AuthenticationResultType.InvalidTenancyName:
                    return "Inquilino inválido";
                case AuthenticationResultType.TenantIsNotActive:
                    return "Inquilino inativo";
                case AuthenticationResultType.UserEmailIsNotConfirmed:
                    return "E-mail não confirmado";
                case AuthenticationResultType.UnauthenticatedUser:
                    return "Usuário não autenticado";
                default:
                    return "Não autorizado";
            }
        }


        private ExternalJwtAuthenticateUserResult CreateExternalUser(User user)
        {

            return new ExternalJwtAuthenticateUserResult()
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Email = user.Email,
                Roles = new string[] { "Admin" },
                Permissions = new string[] { "Admin.Users.Create" }
            };
        }
    }

    public class AuthModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AuthTenantChangeDto
    {
        public Guid? TenantId { get; set; }
        public string OtherValue { get; set; }
    }

    public class AuthRefreshTokenDto
    {
        public Guid? TenantId { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}

