using Mango.Web.Models;

namespace Mango.Web.Services
{
    public class ProductService : IProductService
    {
        public IBaseService _baseService { get; set; }
        public ProductService(IBaseService baseService)
        {
         _baseService= baseService;
            
        }
        public async Task<ResponseDTO> GetAllProductsAsync()
        {
            RequestDTO req = new RequestDTO();
            req.Url = APIType.ProductAPIBaseUrl+"/api/ProductAPI/";
            req.ApiTypeMethod= APIType.APITypeEnum.GET;
            req.ContentType = "application/json";
            
           return  await _baseService.SendAPIRequestAsync(req);
        }

      

        public async Task<ResponseDTO> GetProductByIdAsync(int id)
        {
            RequestDTO req = new RequestDTO();
            req.Url = APIType.ProductAPIBaseUrl + "/api/ProductAPI/GetById/" + id;
            req.ApiTypeMethod = APIType.APITypeEnum.GET;
            req.ContentType = "application/json";

            return await _baseService.SendAPIRequestAsync(req);
        }

        public async Task<ResponseDTO> PostProductDataAsync(ProductDTO productData)
        {

            RequestDTO req = new RequestDTO();
            req.Url = APIType.ProductAPIBaseUrl + "/api/ProductAPI/postproduct/";
            req.ApiTypeMethod = APIType.APITypeEnum.POST;
            req.ContentType = "application/json";
            req.RequestBody = productData;
            return await _baseService.SendAPIRequestAsync(req);
        }

        public async Task<ResponseDTO> PutProductDataAsync(ProductDTO productData)
        {
            RequestDTO req = new RequestDTO();
            req.Url = APIType.ProductAPIBaseUrl + "/api/ProductAPI/updateproduct/";
            req.ApiTypeMethod = APIType.APITypeEnum.PUT;
            req.ContentType = "application/json";
            req.RequestBody = productData;
            return await _baseService.SendAPIRequestAsync(req);
        }
        public async Task<ResponseDTO> DeleteProductAsync(int id)
        {
            RequestDTO req = new RequestDTO();
            req.Url = APIType.ProductAPIBaseUrl + "/api/ProductAPI/deleteproduct/" + id;
            req.ApiTypeMethod = APIType.APITypeEnum.DELETE;
            req.ContentType = "application/json";

            return await _baseService.SendAPIRequestAsync(req);
        }
    }
}
