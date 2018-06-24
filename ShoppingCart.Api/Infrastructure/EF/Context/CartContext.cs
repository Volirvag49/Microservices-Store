using Microsoft.EntityFrameworkCore;
using ShoppingCart.Api.Models;

namespace ShoppingCart.Api.Infrastructure.EF.Context
{
    public class CartContext : DbContext
    {
        public CartContext(DbContextOptions<CartContext> options) : base(options)
        {
        }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<Models.ShoppingCart> ShoppingCarts { get; set; }
    }
}
