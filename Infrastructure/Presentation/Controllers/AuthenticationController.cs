using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared.Dtos.IdentityModule;

namespace Presentation.Controllers
{
    public class AuthenticationController(IServiceManager _serviceManager) : ApiController
    {
        //Post  ==> Register
        [HttpPost("Register")]
        public async Task<ActionResult<UserResultDto>> RegisterAsync(RegisterDto registerDto)
            => Ok( await _serviceManager.AuthenticatioService.RegisterAsync(registerDto));

        //Post  ==> Login
        [HttpPost("Login")]
        public async Task<ActionResult<UserResultDto>> LoginAsync(LoginDto loginDto)
            => Ok( await _serviceManager.AuthenticatioService.LoginAsync(loginDto));

        [Authorize(Roles = "Admin")]
        //Get ==> Get all users
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<UserResultDto>>> GetAllUsersAsync()
            => Ok( await _serviceManager.AuthenticatioService.GetAllUsersAsync());

    }
}
