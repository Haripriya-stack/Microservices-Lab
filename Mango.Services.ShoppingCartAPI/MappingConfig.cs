using AutoMapper;
using Mango.Services.ShoppingCartAPI.Models.DTO;
using Mango.Services.ShoppingCartAPI.Models;

namespace Mango.Services.ShoppingCartAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<CartHeaderDTO, CartHeader>().ReverseMap();
            // CreateMap<Product, ProductDTO >();

            CreateMap<CartDetailsDTO, CartDetails>().ReverseMap(); 
    

        }
    }
}
