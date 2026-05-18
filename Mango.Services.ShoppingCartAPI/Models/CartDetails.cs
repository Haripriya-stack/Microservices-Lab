using Mango.Services.ShoppingCartAPI.Models.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.ShoppingCartAPI.Models
{
    public class CartDetails
    {
        [Key]
        public int CartDetailsId { get; set; }

        public int CartHeaderId { get; set; }

        [ForeignKey(nameof(CartHeaderId))]
        public CartHeader cartHeader { get; set; }

        public int productId { get; set; }
        public int count { get; set; }
        [NotMapped]
        public ProductDTO product { get; set; }
    }
}
