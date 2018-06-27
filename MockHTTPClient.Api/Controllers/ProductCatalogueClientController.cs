using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MockHTTPClient.Api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MockHTTPClient.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/ProductCatalogueClient")]
    public class ProductCatalogueClientController : Controller
    {
        private string hostName = "localhost:50000";

        [HttpGet]
        [Route("/products/{productId}")]
        public async Task<IActionResult> GetById(int productId)
        {
            var productResource = $"/products/{productId}";
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri($"http://{this.hostName}");
                var response = await httpClient.GetAsync(productResource);
                ThrowOnTransientFailure(response);

                var product = JsonConvert.DeserializeObject<Product>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

                if (product != null)
                {
                    return Ok(product);
                }

                return NotFound();

            }
        }

        [HttpPost]
        [Route("/products/search")]
        public async Task<IActionResult> PostSearch(int[] ProductIds)
        {
            var productResource = $"/products/search/";
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri($"http://{this.hostName}");
                var response = await httpClient.PostAsync(productResource, new StringContent(JsonConvert.SerializeObject(ProductIds), Encoding.UTF8, "application/json"));
                ThrowOnTransientFailure(response);

                var result = JsonConvert.DeserializeObject<IEnumerable<Product>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

                if (result != null)
                {
                    return Ok(result);
                }

                return NotFound();
            }
        }

        [HttpPost]
        [Route("/products/add")]
        public async Task<IActionResult> AddItem(Product product)
        {
            var productResource = $"/products/add/";
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri($"http://{this.hostName}");
                var response = await httpClient.PostAsync(productResource, new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json"));
                ThrowOnTransientFailure(response);

                var result = JsonConvert.DeserializeObject<IEnumerable<Product>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

                if (result != null)
                {
                    return Ok(product);
                }

                return NotFound();
            }
        }

        private static void ThrowOnTransientFailure(HttpResponseMessage response)
        {
            if (((int)response.StatusCode) < 200 || ((int)response.StatusCode) > 499) throw new Exception(response.StatusCode.ToString());
        }

        [HttpPut]
        [Route("/products/update")]
        public async Task<IActionResult> UpdateItem([FromBody] Product product)
        {
            var productResource = $"/products/update/";
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri($"http://{this.hostName}");
                var response = await httpClient.PutAsync(productResource, new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json"));
                ThrowOnTransientFailure(response);

                var result = JsonConvert.DeserializeObject<Product>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

                if (result != null)
                {
                    return Ok(product);
                }

                return NotFound();
            }
        }

        [HttpDelete]
        [Route("/products/delete")]
        public async Task<IActionResult> DeleteItem(int? productId)
        {
            var productResource = $"/products/delete/{productId}";
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri($"http://{this.hostName}");
                var response = await httpClient.DeleteAsync(productResource);
                ThrowOnTransientFailure(response);

                var product = JsonConvert.DeserializeObject<IEnumerable<Product>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

                if (product != null)
                {
                    return Ok(product);
                }

                return NotFound();

            }
        }

    }
}