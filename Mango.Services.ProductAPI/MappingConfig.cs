using AutoMapper;
using Mango.Services.ProductAPI.Models.DTO;
using Mango.Services.ProductAPI.Models;

namespace Mango.Services.ProductAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<ProductDTO, Product>();
           // CreateMap<Product, ProductDTO >();

            CreateMap<Product, ProductDTO>()  
    .ForMember(dest => dest.Description,
        opt => opt.MapFrom(src => "UI Viewla" + "-" + src.Description));

        }
    }
}
