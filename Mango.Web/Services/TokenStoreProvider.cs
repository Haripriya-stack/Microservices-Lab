using Mango.Web.Models;

namespace Mango.Web.Services
{
    public class TokenStoreProvider : ITokenStoreProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TokenStoreProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public void ClearToken()
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete(APIType.TokenKeyName);
        }

        public string? GetToken()
        {
            string? token = null;
            bool? isAvailable = _httpContextAccessor.HttpContext?.Request?.Cookies.TryGetValue(APIType.TokenKeyName, out token);
            return isAvailable == true ? token : null;
        }

        public void SetToken(string token)
        {
            _httpContextAccessor.HttpContext?.Response?.Cookies.Append(APIType.TokenKeyName, token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)

            });

        }
    }
}
