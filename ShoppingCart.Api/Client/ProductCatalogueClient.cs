using Newtonsoft.Json;
using Polly;
using ShoppingCart.Api.Client.Interfaces;
using ShoppingCart.Api.Infrastructure.CorrelationToken;
using ShoppingCart.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Api.Client
{
    public class ProductCatalogueClient : IProductCatalogueClient
    {
        private static Policy exponentialRetryPolicy =
          Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, attempt)), (ex, _) => Console.WriteLine(ex.ToString()));

        private static string productCatalogueBaseUrl =
          @"http://localhost:50000/";

        private readonly IHttpClientFactory _httpClientFactory;

        public ProductCatalogueClient (IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogueIds) =>
          exponentialRetryPolicy
            .ExecuteAsync(async () => await GetItemsFromCatalogueService(productCatalogueIds).ConfigureAwait(false));

        private async Task<IEnumerable<ShoppingCartItem>>
          GetItemsFromCatalogueService(int[] productCatalogueIds)
        {
            var response = await
              RequestProductFromProductCatalogue(productCatalogueIds).ConfigureAwait(false);
            return await ConvertToShoppingCartItems(response).ConfigureAwait(false);
        }

        private async Task<HttpResponseMessage> RequestProductFromProductCatalogue(int[] productCatalogueIds)
        {
            var productsResource = "/products/search/";

            var client = _httpClientFactory.Create(new Uri(productCatalogueBaseUrl));

            using (client)
            {
                return await client.PostAsync(productsResource, new StringContent(JsonConvert.SerializeObject(productCatalogueIds), Encoding.UTF8, "application/json"));
            }
        }

        private static async Task<IEnumerable<ShoppingCartItem>> ConvertToShoppingCartItems(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            var products =
              JsonConvert.DeserializeObject<List<ProductCatalogueProduct>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

            var result = new List<ShoppingCartItem>();

            foreach (var item in products)
            {
                result.Add(              
                        new ShoppingCartItem()
                        {
                            //Id = item.ProductId,
                            ProductName = item.ProductName,
                            Description = item.ProductDescription,
                            Price = 12
                        }
                    );
            }
            return result;
        }

        private class ProductCatalogueProduct
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public string ProductDescription { get; set; }
           // public decimal Price { get; set; }
        }
    }
}
