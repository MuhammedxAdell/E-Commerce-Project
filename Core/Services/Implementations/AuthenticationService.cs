using Domain.Entities.IdentityModule;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Services.Abstraction.Contracts;
using Shared.Dtos.IdentityModule;
using System.ComponentModel.DataAnnotations;
using ValidationException = Domain.Exceptions.ValidationException;

namespace Services.Implementations
{
    public class AuthenticationService(UserManager<User> _userManager) : IAuthenticationService
    {
        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
            //Email exists ?
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null) throw new UnauthorizedException();
            //Password is correct ?
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid) throw new UnauthorizedException();
            return new UserResultDto(user.DisplayName, "Token", user.Email);
        }
            

        public async Task<UserResultDto> RegisterAsync(RegisterDto registerDto)
        {
            var user = new User
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                throw new ValidationException(errors);
            }
            return new UserResultDto(user.DisplayName, "Token", user.Email);
        }
    }
}
