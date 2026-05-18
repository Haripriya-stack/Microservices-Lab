using Mango.Services.ShoppingCartAPI.Models.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.ShoppingCartAPI.Models.DTO
{
    public class CartDTO
    {
       
       public CartHeaderDTO? cartHeader { get; set; }

        public IEnumerable<CartDetailsDTO>? cartDetailsList { get; set; }



    }
}
