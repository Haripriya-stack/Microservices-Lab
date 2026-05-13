using Mango.Web.Models;
using Mango.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        public IAuthService _authService { get; set; }
        public AuthController(IAuthService authService) { _authService = authService; }
       
        public async Task<IActionResult> Logout()
        {
            return View();
        }
        public async Task<IActionResult> Login()
        {
                return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO loginDTO)
        {
            if (ModelState.IsValid)
            {
                var response = await _authService.LoginAsync(loginDTO);

                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    ModelState.AddModelError("CustomError", response.Message);
                }

            }

            return View();
        }
        public async Task<IActionResult> Register()
        {


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationDTO regDTO)
        {
            if (ModelState.IsValid)
            { 
              var response= await _authService.RegisterAsync(regDTO);
                if(response.IsSuccess)
                {
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    ModelState.AddModelError("CustomError", response.Message);
                }

            }

            return View();
        }
    }
}
