using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Api.Models
{
    public class ShoppingCart 
    {
        [Required]
        public int? Id { get; set; }
        public int UserId { get; set; }
        public ICollection<ShoppingCartItem> Items { get; set; }
    }
}
