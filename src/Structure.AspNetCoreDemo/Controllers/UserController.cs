using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Structure.AspNetCore;
using Structure.AspNetCoreDemo.Application;
using Structure.Tests.Shared.Dtos;
using Structure.Tests.Shared.Entities;
using System;
using System.Threading.Tasks;

namespace Structure.AspNetCoreDemo.Controllers
{
    public class UserController : CrudApiController<User, Guid, UserDto, IUserAppService>
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserController(IUserAppService appService, IHttpContextAccessor httpContextAccessor) : base(appService)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> GetRegister()
        {
            await appService.Register("Danilo Singh");
            return Ok();
        }
    }
}
