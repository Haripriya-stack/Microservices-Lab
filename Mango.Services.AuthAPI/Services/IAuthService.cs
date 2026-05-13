using Mango.Services.AuthAPI.Models.DTO;

namespace Mango.Services.AuthAPI.Services
{
    public interface IAuthService
    {
        public  Task<LoginResponseDTO> LoginUser(LoginRequestDTO loginRequestDTO);
        public  Task<string> RegisterUser(RegistrationDTO registrationDTO);

        public Task<bool> AssignRoleToUser(string email, string roleName);
    }
}
