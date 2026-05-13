using Mango.Web.Models;
using Microsoft.AspNetCore.Identity.Data;

namespace Mango.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;

        public AuthService(IBaseService baseService)
        {
                _baseService= baseService;
        }
        public async Task<ResponseDTO> AssignRoleToUserAsync(RegistrationDTO registrationRequest)
        {

            RequestDTO req = new RequestDTO();
            req.Url = APIType.AuthAPIBaseUrl + "/api/AuthAPI/AssignRole";
            req.ApiTypeMethod = APIType.APITypeEnum.POST;
            req.ContentType = "application/json";
            req.RequestBody = registrationRequest;
            return await _baseService.SendAPIRequestAsync(req);
        }

        public async Task<ResponseDTO> LoginAsync(LoginRequestDTO loginRequest)
        {
            RequestDTO req = new RequestDTO();
            req.Url = APIType.AuthAPIBaseUrl + "/api/AuthAPI/login";
            req.ApiTypeMethod = APIType.APITypeEnum.POST;
            req.ContentType = "application/json";
            req.RequestBody = loginRequest;
            return await _baseService.SendAPIRequestAsync(req);
        }

        public async Task<ResponseDTO> RegisterAsync(RegistrationDTO registrationRequest)
        {
            RequestDTO req = new RequestDTO();
            req.Url = APIType.AuthAPIBaseUrl + "/api/AuthAPI/register";
            req.ApiTypeMethod = APIType.APITypeEnum.POST;
            req.ContentType = "application/json";
            req.RequestBody = registrationRequest;
            return await _baseService.SendAPIRequestAsync(req);
        }
    }
}
