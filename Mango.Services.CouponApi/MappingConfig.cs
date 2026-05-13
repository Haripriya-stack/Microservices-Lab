using AutoMapper;
using Mango.Services.CouponApi.Models.DTO;
using Mango.Services.CouponApi.Models;
namespace Mango.Services.CouponApi
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<CouponDTO, Coupon>();
           // CreateMap<Coupon, CouponDTO>();

            CreateMap<Coupon, CouponDTO>()
    .ForMember(dest => dest.CouponCode,
        opt => opt.MapFrom(src => "UI Viewla" + "-" + src.CouponCode));

        }
    }
}
