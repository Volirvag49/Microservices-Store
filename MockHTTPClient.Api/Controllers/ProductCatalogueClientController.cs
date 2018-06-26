using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MockHTTPClient.Api.Models;
using Newtonsoft.Json;

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
            var userResource = $"/products/{productId}";
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri($"http://{this.hostName}");
                var response = await httpClient.GetAsync(userResource);
                ThrowOnTransientFailure(response);

                var product = JsonConvert.DeserializeObject<Product>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

                if (product != null)
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

    }
}