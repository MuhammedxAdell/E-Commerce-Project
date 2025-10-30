using Domain.Entities.IdentityModule;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Abstraction.Contracts;
using Shared.Common;
using Shared.Dtos.IdentityModule;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ValidationException = Domain.Exceptions.ValidationException;

namespace Services.Implementations
{
    public class AuthenticationService(UserManager<User> _userManager , IOptions<JwtOption> _options) : IAuthenticationService
    {
        public async Task<IEnumerable<UserResultDto>> GetAllUsersAsync()
        {
           
            var users = _userManager.Users.Select(user => new UserResultDto
            (
                user.DisplayName,
                //The token should be hidden
                "",
                user.Email
            ));
            return await users.ToListAsync();
        }

        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
            //Email exists ?
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null) throw new UnauthorizedException();
            //Password is correct ?
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid) throw new UnauthorizedException();
            return new UserResultDto(user.DisplayName, await CreateTokenAsync(user), user.Email);
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
            return new UserResultDto(user.DisplayName, await CreateTokenAsync(user), user.Email);
        }

        //Token ==> encrypted string
        
        //Helper Method
        private async Task<string> CreateTokenAsync( User user)
        {
            var jwtOptions = _options.Value;
            //Claims
            //Name  , Email ,roles [ m - m ]
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name , user.DisplayName),
                new Claim(ClaimTypes.Email , user.Email)
            };
            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            //Secret Key ==> Symmetric Security Key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));

            //Algorithm [ Algorithem + Key ] ==> Signing Credentials
            var signInCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer: jwtOptions.Issuer, audience: jwtOptions.Audience, claims: claims, expires: DateTime.UtcNow.AddDays(jwtOptions.ExpirationInDays) , signingCredentials: signInCredentials);

            //Write Token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
