using Mango.Web.Models;

namespace Mango.Web.Services
{
    public class CouponService : ICouponService
    {
        public IBaseService _baseService { get; set; }
        public CouponService(IBaseService baseService)
        {
         _baseService= baseService;
            
        }
        public async Task<ResponseDTO> GetAllCouponsAsync()
        {
            RequestDTO req = new RequestDTO();
            req.Url = APIType.CouponAPIBaseUrl+"/api/CouponAPI/";
            req.ApiTypeMethod= APIType.APITypeEnum.GET;
            req.ContentType = "application/json";
            
           return  await _baseService.SendAPIRequestAsync(req);
        }

        public async Task<ResponseDTO> GetCouponByCodeAsync(string code)
        {
            RequestDTO req = new RequestDTO();
            req.Url = APIType.CouponAPIBaseUrl + "/api/CouponAPI/GetByCode/"+code;
            req.ApiTypeMethod = APIType.APITypeEnum.GET;
            req.ContentType = "application/json";

            return await _baseService.SendAPIRequestAsync(req);
        }

        public async Task<ResponseDTO> GetCouponByIdAsync(int id)
        {
            RequestDTO req = new RequestDTO();
            req.Url = APIType.CouponAPIBaseUrl + "/api/CouponAPI/GetById/" + id;
            req.ApiTypeMethod = APIType.APITypeEnum.GET;
            req.ContentType = "application/json";

            return await _baseService.SendAPIRequestAsync(req);
        }

        public async Task<ResponseDTO> PostCouponDataAsync(CouponDTO couponData)
        {

            RequestDTO req = new RequestDTO();
            req.Url = APIType.CouponAPIBaseUrl + "/api/CouponAPI/postcoupon/";
            req.ApiTypeMethod = APIType.APITypeEnum.POST;
            req.ContentType = "application/json";
            req.RequestBody = couponData;
            return await _baseService.SendAPIRequestAsync(req);
        }

        public async Task<ResponseDTO> PutCouponDataAsync(CouponDTO couponData)
        {
            RequestDTO req = new RequestDTO();
            req.Url = APIType.CouponAPIBaseUrl + "/api/CouponAPI/updatecoupon/";
            req.ApiTypeMethod = APIType.APITypeEnum.PUT;
            req.ContentType = "application/json";
            req.RequestBody = couponData;
            return await _baseService.SendAPIRequestAsync(req);
        }
        public async Task<ResponseDTO> DeleteCouponAsync(int id)
        {
            RequestDTO req = new RequestDTO();
            req.Url = APIType.CouponAPIBaseUrl + "/api/CouponAPI/deletecoupon/" + id;
            req.ApiTypeMethod = APIType.APITypeEnum.DELETE;
            req.ContentType = "application/json";

            return await _baseService.SendAPIRequestAsync(req);
        }
    }
}
