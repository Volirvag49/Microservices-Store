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
        private static List<Product> _products = new List<Product>
        {
                new Product(){ProductId = 1 , ProductName = "Продукт 1", ProductDescription = "Описание 1", Price = 10},
                new Product(){ProductId = 2 , ProductName = "Продукт 2", ProductDescription = "Описание 2", Price = 20},
                new Product(){ProductId = 3 , ProductName = "Продукт 3", ProductDescription = "Описание 3", Price = 30},
                new Product(){ProductId = 4 , ProductName = "Продукт 4", ProductDescription = "Описание 4", Price = 40},
                new Product(){ProductId = 5 , ProductName = "Продукт 5", ProductDescription = "Описание 5", Price = 50}
        };

        [HttpGet]
        [Route("/products")]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public  IActionResult GetAll()
        {
            if (_products != null)
            {
                return Ok(_products.OrderBy(p => p.ProductId));
            }

            return NotFound();
        }

        [HttpGet]
        [Route("/products/{productId}")]
        public  IActionResult GetById(int productId)
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
        public IActionResult Search([FromQuery]int[] ProductIds)
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
                return Ok(searchResult.OrderBy(p => p.ProductId));
            }

            return NotFound();
        }

        [HttpPost]
        [Route("/products/search")]
        public IActionResult PostSearch([FromBody]int[] ProductIds)
        {
            if (ProductIds.Count() == 0)
            {
                return BadRequest();
            }

            var searchResult = new List<Product>();
            foreach (var item in ProductIds)
            {
                var product = _products.SingleOrDefault(p => p.ProductId == item);
                searchResult.Add(product);
            }
            if (searchResult != null)
            {
                return Ok(searchResult.OrderBy(p => p.ProductId));
            }

            return NotFound();
        }

        [HttpPost]
        [Route("/products/add")]
        public IActionResult AddItem(Product product)
        {
            if (product != null)
            {
                _products.Add(product);

                return Ok(_products.OrderBy(p => p.ProductId).ToList());
            }

            return NotFound();
        }

        [HttpPut]
        [Route("/products/update")]
        public IActionResult UpdateItem([FromBody] Product product)
        {
            if (product != null)
            {
               var updatedProduct = _products.SingleOrDefault(p => p.ProductId == product.ProductId);
                if (updatedProduct != null)
                {
                    updatedProduct.ProductName = product.ProductName;
                    updatedProduct.ProductDescription = product.ProductDescription;
                    updatedProduct.Price = product.Price;
                }

                else return NotFound();
               
                return Ok(product);
            }

            return NotFound();
        }

        [HttpDelete]
        [Route("/products/delete/{productId}")]
        public IActionResult DeleteItem(int? productId)
        {
            if (productId != null)
            {
                var deleteddProduct = _products.SingleOrDefault(p => p.ProductId == productId);
                if (deleteddProduct != null)
                {
                    _products.Remove(deleteddProduct);

                }

                else return NotFound();

                return Ok(_products.OrderBy(p => p.ProductId).ToList());
            }

            return NotFound();
        }
    }
}