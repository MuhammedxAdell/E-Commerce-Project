using Shared.Dtos.IdentityModule;
using Shared.Dtos.OrderModule;

namespace Services.Abstraction.Contracts
{
    public interface IAuthenticationService
    {
        //Login     ==> return UserResultDto [ DisplayName , Token , Email ] ==> Take Params [Email , Password ]
        Task<UserResultDto> LoginAsync(LoginDto loginDto);
        //Register  ==> return UserResultDto [ DisplayName , Token , Email ] ==> Take Params [ DisplayName , Email , Password , PhoneNumber , UserName ]
        Task<UserResultDto> RegisterAsync(RegisterDto registerDto);
        //Get All Users ==> return List of UserResultDto [ DisplayName , Token , Email ]
        Task<IEnumerable<UserResultDto>> GetAllUsersAsync();

        //Get Current user
        Task<UserResultDto> GetCurrentUserAsync(string userEmail);
        //Check if email exist
        Task<bool> CheckEmailExistAsync(string userEmail);
        //Get Address
        Task<AddressDto> GetUserAddressAsync(string userEmail);
        //Update Address
        Task<AddressDto> UpdateUserAddressAsync( string userEmail , AddressDto addressDto);

    }
}
