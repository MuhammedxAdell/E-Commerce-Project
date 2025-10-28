using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.IdentityModule
{
    public class User : IdentityUser
    {
        public string DisplayName { get; set; }
        public Address Addrees { get; set; }
    }
}
