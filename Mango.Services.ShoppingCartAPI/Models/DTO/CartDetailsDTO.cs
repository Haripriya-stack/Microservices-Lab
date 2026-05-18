using Mango.Services.ShoppingCartAPI.Models.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.ShoppingCartAPI.Models.DTO
{
    public class CartDetailsDTO
    {
       
        public int CartDetailsId { get; set; }

        public int CartHeaderId { get; set; }

        public CartHeaderDTO? cartHeader { get; set; }

        public int productId { get; set; }

        public int count { get; set; }
        public ProductDTO? product { get; set; }
    }
}
