using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Mango.Services.AuthAPI.Services
{
    public class AuthService : IAuthService
    {
        public UserManager<ApplicationUser> _userManager { get; set; }

        public RoleManager<IdentityRole>   _roleManager { get; set; }

        public AppDBContext _dbContext { get; set; }
        public JWTOptions _jwtsettings { get; set; }
        public AuthService(AppDBContext dBContext,UserManager<ApplicationUser> appuser,
            RoleManager<IdentityRole> userrole,IOptions<JWTOptions> jwtsettings)
         {
           _userManager=appuser;
            _roleManager=userrole;
            _dbContext=dBContext;
            _jwtsettings = jwtsettings.Value;

        }
        

        public async Task<string> RegisterUser(RegistrationDTO registrationDTO)
        {
            try
            {
                var appuser = new ApplicationUser()
                {
                    UserName = registrationDTO.Email,
                    Email = registrationDTO.Email,
                    PhoneNumber = registrationDTO.PhoneNumber,
                    FullName = registrationDTO.Name,
                    NormalizedEmail = registrationDTO.Password+"--"+ registrationDTO.Email.Normalize(),
                    NormalizedUserName = registrationDTO.Name.Normalize()

                };

                IdentityResult res = await _userManager.CreateAsync(appuser, registrationDTO.Password);

                if (res.Succeeded)
                {
                    var foundUser= _dbContext.ApplicationUsers.First(u=>u.UserName==registrationDTO.Email);
                    if(foundUser is not null)
                    {
                        new UserDTO()
                        {
                            Email = foundUser.Email,
                            Name = foundUser.UserName,
                            PhoneNumber = foundUser.PhoneNumber,
                            UserID = foundUser.Id
                        };
                        return "";
                    }
                }

                else
                {
                    return res.Errors.FirstOrDefault().Description.ToString();
                }
            }
            catch (Exception ex)
            {
            }
            return "Error Encountered";

        }
        public async Task<LoginResponseDTO> LoginUser(LoginRequestDTO loginRequestDTO)
        {
            try
            {
                var user = _dbContext.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower());
                bool isValidUser = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
                if (user == null || isValidUser == false)
                {
                    return new LoginResponseDTO() { Token = "", User = null, message = (!isValidUser) ? "No matching password" : "No matching username" };

                }
                else
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var token = GenerateToken(user, roles);
                    UserDTO _user = new UserDTO()
                    {
                        Email = user.Email,
                        Name = user.FullName,
                        UserID = user.Id,
                        PhoneNumber = user.PhoneNumber
                    };
                    return new LoginResponseDTO() { Token = token, User = _user };
                }
            }
            catch (Exception ex)
            {

            }
            return new LoginResponseDTO()
            {
                Token = "",
                User = null
            };
        }

        public async Task<bool> AssignRoleToUser(string email, string roleName)
        {
            var user = _dbContext.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == email.ToLower());
            if(user == null)
            {
                return false;
            }
            else
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    IdentityResult res = await _roleManager.CreateAsync(new IdentityRole() { Name = roleName });



                    if (!res.Succeeded)
                    {
                        return false;
                    }
                }

                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
        }
        public string GenerateToken(ApplicationUser appuser, IEnumerable<string> roles)
        {
            // var secret = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var secret = _jwtsettings.Secret;
            var key= Encoding.UTF8.GetBytes(secret);

            var tokenHandler = new JwtSecurityTokenHandler();

            var claimList = new List<Claim>()
                {
                 new Claim(JwtRegisteredClaimNames.Email, appuser.Email),
                 new Claim(JwtRegisteredClaimNames.Sub, appuser.Id),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 new Claim("uid", appuser.Id),
                 new Claim(JwtRegisteredClaimNames.Name, appuser.FullName),
                 new Claim(JwtRegisteredClaimNames.PhoneNumber, appuser.PhoneNumber),
              

                };

            claimList.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
            var tokenDecriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claimList),
                Issuer = _jwtsettings.Issuer,
                Audience = _jwtsettings.Audience,
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_jwtsettings.DurationInMinutes)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

            };
            var token = tokenHandler.CreateToken(tokenDecriptor);
            return tokenHandler.WriteToken(token);

        }

      
    }
}
