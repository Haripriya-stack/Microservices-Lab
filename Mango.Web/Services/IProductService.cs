using Mango.Web.Models;

namespace Mango.Web.Services
{
    public interface IProductService
    {
        public Task<ResponseDTO> GetAllProductsAsync();

        public Task<ResponseDTO> GetProductByIdAsync(int id);

      

        public  Task<ResponseDTO> PostProductDataAsync(ProductDTO productData);
        public Task<ResponseDTO> PutProductDataAsync(ProductDTO productData);
        public Task<ResponseDTO> DeleteProductAsync(int id);
    }
}
