using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Api.Client.Interfaces;
using ShoppingCart.Api.Infrastructure.EF.Context;

namespace ShoppingCart.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/ShoppingCart")]
    public class ShoppingCartController : Controller
    {
        private readonly CartContext  _cartContext;
        private readonly IProductCatalogueClient _productCatalogueClient;

        public ShoppingCartController(
            CartContext cartContext
            ,IProductCatalogueClient productCatalogueClient
            )
        {
            _cartContext = cartContext;
            _productCatalogueClient = productCatalogueClient;
        }

        [HttpGet]
        [Route("/items")]
        [ProducesResponseType(typeof(Models.ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetShoppingCart([FromQuery]int userId)
        {
            if (userId <= 0)
            {
                return BadRequest();
            }

            var shoppingCart = await _cartContext.ShoppingCarts
                .Include(b => b.Items)
                .SingleOrDefaultAsync(ci => ci.UserId == userId);


            if (shoppingCart != null)
            {

                return Ok(shoppingCart);
            }

            return NotFound();
        }

        [HttpPost]
        [Route("{userId}/items")]
        [ProducesResponseType(typeof(Models.ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddItemsToCart(int userId, [FromBody]int[] productIds)
        {
            if (userId <= 0)
            {
                return BadRequest();
            }

            var shoppingCart = await _cartContext.ShoppingCarts
               .Include(b => b.Items)
               .SingleOrDefaultAsync(ci => ci.UserId == userId);

            if (shoppingCart == null)
            {
                shoppingCart = new Models.ShoppingCart()
                {
                    UserId = userId
                };

                var result =  _cartContext.ShoppingCarts.Add(shoppingCart);

                await _cartContext.SaveChangesAsync();
            }

            var shoppingCartItems = await _productCatalogueClient.GetShoppingCartItems(productIds).ConfigureAwait(false);


            if(shoppingCartItems.Count() > 0)
            {
                foreach (var item in shoppingCartItems)
                {

                    var cartItem = _cartContext.ShoppingCartItems.FirstOrDefault(i => i.Id == item.Id);

                    if (cartItem == null)
                    {
                        item.ShoppingCartId = shoppingCart.Id;
                        _cartContext.ShoppingCartItems.Add(item);
                    }
                }
            }

            await _cartContext.SaveChangesAsync();

            return Ok(shoppingCart);
        }


        [HttpDelete]
        [Route("{userId}")]
        [ProducesResponseType(typeof(Models.ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteItemsFromCart(int userId, [FromBody]int[] productIds)
        {
            if (userId <= 0)
            {
                return BadRequest();
            }

            var shoppingCart = await _cartContext.ShoppingCarts
               .Include(b => b.Items)
               .SingleOrDefaultAsync(ci => ci.UserId == userId);

            foreach (var id in productIds)
            {
                var item = shoppingCart.Items.Where(ci => ci.Id == id).FirstOrDefault();

                _cartContext.Remove(item);
            }

            await _cartContext.SaveChangesAsync();

            return Ok(shoppingCart);
        }
    }
}