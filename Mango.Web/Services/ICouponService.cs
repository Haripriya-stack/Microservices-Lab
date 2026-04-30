using Mango.Web.Models;

namespace Mango.Web.Services
{
    public interface ICouponService
    {
        public Task<ResponseDTO> GetAllCouponsAsync();

        public Task<ResponseDTO> GetCouponByCodeAsync(string code);

        public Task<ResponseDTO> GetCouponByIdAsync(int id);

        public  Task<ResponseDTO> PostCouponDataAsync(CouponDTO couponData);
        public Task<ResponseDTO> PutCouponDataAsync(CouponDTO couponData);
        public Task<ResponseDTO> DeleteCouponAsync(int id);
    }
}
