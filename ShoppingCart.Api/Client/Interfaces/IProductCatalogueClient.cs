using ShoppingCart.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingCart.Api.Client.Interfaces
{
    public interface IProductCatalogueClient
    {
        Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogueIds);
    }
}
