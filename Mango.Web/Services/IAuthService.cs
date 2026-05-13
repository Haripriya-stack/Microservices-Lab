using Mango.Web.Models;

namespace Mango.Web.Services
{
    public interface IAuthService
    {
        public Task<ResponseDTO> LoginAsync(LoginRequestDTO loginRequest);
         public Task<ResponseDTO> RegisterAsync(RegistrationDTO registrationRequest);
        public Task<ResponseDTO> AssignRoleToUserAsync(RegistrationDTO registrationRequest);
    }
}
