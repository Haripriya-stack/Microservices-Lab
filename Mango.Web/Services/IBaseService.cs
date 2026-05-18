using Mango.Web.Models;

namespace Mango.Web.Services
{
    public interface IBaseService
    { 
        Task<ResponseDTO> SendAPIRequestAsync(RequestDTO request,bool withBearer=true);
    }
}
