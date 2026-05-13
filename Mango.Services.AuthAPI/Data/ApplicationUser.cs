using Microsoft.AspNetCore.Identity;

namespace Mango.Services.AuthAPI.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
