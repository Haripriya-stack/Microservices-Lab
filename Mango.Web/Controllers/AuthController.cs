using Mango.Web.Models;
using Mango.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        public IAuthService _authService { get; set; }
        public ITokenStoreProvider _tokenStoreProvider { get; set; }
      
        public AuthController(IAuthService authService,ITokenStoreProvider tokenStoreProvider
           )
        { _authService = authService; _tokenStoreProvider = tokenStoreProvider;
      
        }
       
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _tokenStoreProvider.ClearToken();
            return RedirectToAction("Index", "Home");
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
                 var loginResponse = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(response.Result));
                    
                    TempData["Result"] =
                    JsonConvert.SerializeObject(loginResponse);

                    _tokenStoreProvider.SetToken(loginResponse?.Token);
                    
                    await SigninUser(loginResponse);

                    TempData["SuccessMessage"] = "User Loggedin successfully!";

                    return RedirectToAction("Success","Coupon");
                }
                else
                {
                    TempData["ErrorMessage"] = "Please Login with Correct Details";
                    ModelState.AddModelError("CustomError", response.Message);
                }

            }

            return View(loginDTO);
        }
        public async Task<IActionResult> Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem(){Text=APIType.AdminRole,Value=APIType.AdminRole},
                new SelectListItem(){Text=APIType.CustomerRole,Value=APIType.CustomerRole}
            };
            ViewBag.Roles = roleList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationDTO regDTO)
        {
            if (ModelState.IsValid)
            {
                var response = await _authService.RegisterAsync(regDTO);
                if (response.IsSuccess)
                {
                    if (regDTO.RoleName == null)
                    {
                        ViewBag.Message = "Role autoassigned as Customer";
                        regDTO.RoleName = APIType.CustomerRole;
                    }
                        var result = await _authService.AssignRoleToUserAsync(regDTO);
                        if (result.IsSuccess)
                        {
                            TempData["SuccessMessage"] = "User registered successfully!";
                           

                            return RedirectToAction(nameof(Login));
                        }
                        else
                        {
                            TempData["ErrorMessage"] = response.Message ?? "An error occurred while registering the userS.";
                            ModelState.AddModelError("CustomError", response.Message);
                        }
                    

                }
                else
                {
                    TempData["ErrorMessage"] = response.Message ?? "An error occurred while registering the userS.";
                    ModelState.AddModelError("CustomError", response.Message);
                }


            }
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem(){Text=APIType.AdminRole,Value=APIType.AdminRole},
                new SelectListItem(){Text=APIType.CustomerRole,Value=APIType.CustomerRole}
            };
            ViewBag.Roles = roleList;

            return View(regDTO);
        }


        private async Task SigninUser(LoginResponseDTO loginResponse)
        {
            // Implement the logic to sign in the user using the loginResponse data
            // This may involve creating claims, setting cookies, etc.
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwttoken= tokenHandler.ReadJwtToken(loginResponse.Token);

            List<Claim> claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, jwttoken.Claims.First(c=> c.Type== JwtRegisteredClaimNames.Email).Value),
                 new Claim(JwtRegisteredClaimNames.Sub, jwttoken.Claims.First(c=> c.Type== JwtRegisteredClaimNames.Sub).Value),
                 new Claim(JwtRegisteredClaimNames.Jti, jwttoken.Claims.First(c=> c.Type== JwtRegisteredClaimNames.Jti).Value),
                 new Claim("uid", jwttoken.Claims.First(c=> c.Type== "uid").Value),
                 new Claim(JwtRegisteredClaimNames.Name, jwttoken.Claims.First(c=> c.Type== JwtRegisteredClaimNames.Name).Value),
                new Claim(JwtRegisteredClaimNames.PhoneNumber, jwttoken.Claims.First(c=> c.Type== JwtRegisteredClaimNames.PhoneNumber).Value),
                  new Claim(ClaimTypes.Role, jwttoken.Claims.First(c=> c.Type== "role").Value),
                      new Claim(ClaimTypes.Name, jwttoken.Claims.First(c=> c.Type== JwtRegisteredClaimNames.Name).Value),

            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal=new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
