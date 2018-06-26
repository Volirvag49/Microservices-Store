using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mock.Api.Models;

namespace Mock.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/ProductCatalogue")]
    public class ProductCatalogueController : Controller
    {
        private List<Product> _products;

        public ProductCatalogueController()
        {
            _products = new List<Product>()
            {
                new Product(){ProductId = 1 , ProductName = "Продукт 1", ProductDescription = "Описание 1", Price = 10},
                new Product(){ProductId = 2 , ProductName = "Продукт 2", ProductDescription = "Описание 2", Price = 20},
                new Product(){ProductId = 3 , ProductName = "Продукт 3", ProductDescription = "Описание 3", Price = 30},
                new Product(){ProductId = 4 , ProductName = "Продукт 4", ProductDescription = "Описание 4", Price = 40},
                new Product(){ProductId = 5 , ProductName = "Продукт 5", ProductDescription = "Описание 5", Price = 50}
            };
        }

        [HttpGet]
        [Route("/products")]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            if (_products != null)
            {
                return Ok(_products);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("/products/{productId}")]
        public async Task<IActionResult> GetById(int productId)
        {
            if (productId != 0)
            {
                return Ok(_products.SingleOrDefault(p => p.ProductId == productId));
            }

            return NotFound();
        }

        [HttpGet]
        [Route("/products2")]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Search([FromQuery]int[] ProductIds)
        {
            if (ProductIds.Count() == 0)
            {
                return BadRequest();
            }

            var searchResult = new List<Product>();           
            foreach (var item in ProductIds)
            {
               var product =  _products.SingleOrDefault(p => p.ProductId == item);
                searchResult.Add(product);
            }
            if (searchResult != null)
            {
                return Ok(searchResult);
            }

            return NotFound();
        }
    }
}