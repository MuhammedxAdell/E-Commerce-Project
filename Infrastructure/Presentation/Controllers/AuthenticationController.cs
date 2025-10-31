using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared.Dtos.IdentityModule;
using Shared.Dtos.OrderModule;
using System.Security.Claims;

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

        [HttpGet("EmailExist")]
        public async Task<ActionResult<bool>> CheckEmailExistAsync( string userEmail)
            => Ok( await _serviceManager.AuthenticatioService.CheckEmailExistAsync(userEmail));

        [Authorize]
        //Get ==> Get current user
        [HttpGet]
        public async Task<ActionResult<UserResultDto>> GetCurrentUserAsync()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await _serviceManager.AuthenticatioService.GetCurrentUserAsync(userEmail);
            return Ok(user);
        }

        [Authorize]
        //Get ==> Get user address
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetUserAddressAsync()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var address = await _serviceManager.AuthenticatioService.GetUserAddressAsync(userEmail);
            return Ok(address);
        }

        [Authorize]
        //Put ==> Update user address
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddressAsync(AddressDto addressDto)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var updatedAddress = await _serviceManager.AuthenticatioService.UpdateUserAddressAsync(userEmail, addressDto);
            return Ok(updatedAddress);
        }

    }
}
