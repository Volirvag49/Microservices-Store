using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Api.Models
{
    public class ShoppingCartItem
    {
        [Required]
        public int? Id { get; set; }
        [Required]
        [MaxLength(20, ErrorMessage = "Превышена максимальная длина записи")]
        public string ProductName { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }

        public int? ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
    }
}
