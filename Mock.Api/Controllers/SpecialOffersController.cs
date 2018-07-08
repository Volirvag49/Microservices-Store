using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mock.Api.Infrastructure.EventFeed;
using Mock.Api.Models.ProductCatalogue;
using Mock.Api.Models.SpecialOffers;

namespace Mock.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/SpecialOffersa/EventsFeed")]
    public class SpecialOffersController : Controller
    {
        private static List<SpecialOffer> _products = new List<SpecialOffer>
        {
                new SpecialOffer(){ProductId = 1 , ProductName = "Продукт 1", ProductDescription = "Описание 1", Price = 9, Update = "Скидка 10%"},
                new SpecialOffer(){ProductId = 2 , ProductName = "Продукт 2", ProductDescription = "Описание 2", Price = 19, Update = "Скидка 20%"},
                new SpecialOffer(){ProductId = 3 , ProductName = "Продукт 3", ProductDescription = "Описание 3", Price = 29, Update = "Скидка 30%"},
                new SpecialOffer(){ProductId = 4 , ProductName = "Продукт 4", ProductDescription = "Описание 4", Price = 39, Update = "Скидка 40%"},
                new SpecialOffer(){ProductId = 5 , ProductName = "Продукт 5", ProductDescription = "Описание 5", Price = 49, Update = "Скидка 50%"}
        };
        private static EventStore _eventStore = new EventStore();

        public SpecialOffersController()
        {
            foreach (var item in _products)
            {
                _eventStore.Raise("Специальное предложение", item);
            }
        }

        [HttpGet]
        [Route("/Events")]
        [ProducesResponseType(typeof(IEnumerable<Event>), (int)HttpStatusCode.OK)]
        public IActionResult GetEvents([FromQuery]long firstEventSequenceNumber, [FromQuery]long lastEventSequenceNumber)
        {
            var events = _eventStore.GetEvents(firstEventSequenceNumber, lastEventSequenceNumber);
            if (events != null)
            {
                return Ok(events);
            }

            return NotFound();
        }
    }
}