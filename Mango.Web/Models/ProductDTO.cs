using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Web.Models
{
    public class ProductDTO
    {
     
        public int ProductId { get; set; }

        public string? Name { get; set; }
     
        public double Price { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? CategoryName { get; set; }

        public int NoOfItems { get; set; } = 1;
    }
}
