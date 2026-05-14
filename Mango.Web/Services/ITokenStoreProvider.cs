namespace Mango.Web.Services
{
    public interface ITokenStoreProvider
    {
        void SetToken(string token);
         string? GetToken();

        void ClearToken();
    }
}
