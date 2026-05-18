using Mango.Services.AuthAPI.Models.DTO;
using Mango.Services.AuthAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        public IAuthService _authService { get; set; }
        public ResponseDTO responseDTO { get; set; }
        public AuthAPIController(IAuthService authService)
        {
            _authService=authService;
            responseDTO = new();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationDTO registrationDTO)
        {
            string result = await _authService.RegisterUser(registrationDTO);

            if (!string.IsNullOrEmpty(result))
            {

                responseDTO.Message = result;
                responseDTO.IsSuccess = false;
                return BadRequest(responseDTO);
            }

           //   RedirectToAction(nameof(AssignRole), registrationDTO);

              return Ok(responseDTO);
        }

        public async Task<IActionResult> Unregister()
        {
            return Ok();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
           
               LoginResponseDTO loginResponseDTO=await _authService.LoginUser(loginRequestDTO);
               if(loginResponseDTO.User !=null)
                {
                    responseDTO.IsSuccess = true;
                    responseDTO.Result = loginResponseDTO;
                    responseDTO.Message = loginResponseDTO.message;
                    return Ok(responseDTO);
                }
               responseDTO.IsSuccess = false;
                responseDTO.Message= loginResponseDTO.message;
                return BadRequest(responseDTO);
            
            
           
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole(RegistrationDTO registrationDTO)
        {

            bool result = await _authService.AssignRoleToUser(registrationDTO.Email, registrationDTO.RoleName);
            if (result)
            {
                responseDTO.IsSuccess = true;
                responseDTO.Message = "Role assigned successfully";
                return Ok(responseDTO);
            }
            responseDTO.IsSuccess = false;
            responseDTO.Message = "Failed to assign role";
            return BadRequest(responseDTO);



        }

    }
}
