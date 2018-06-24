using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Api.Infrastructure.EventFeed;
using ShoppingCart.Api.Infrastructure.EventFeed.Interfaces;

namespace ShoppingCart.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/EventsFeed")]
    public class EventsFeedController : Controller
    {
        private readonly IEventStore _eventStore;
        public EventsFeedController(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        [HttpGet]
        [Route("/Events")]
        [ProducesResponseType(typeof(IEnumerable<Event>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetEvents([FromQuery]long firstEventSequenceNumber, [FromQuery]long lastEventSequenceNumber)
        {
            if (firstEventSequenceNumber <= 0 || lastEventSequenceNumber <= 0)
            {
                return BadRequest();
            }

            var events = _eventStore.GetEvents(firstEventSequenceNumber, lastEventSequenceNumber);
            if (events != null)
            {
                return Ok(events);
            }

            return NotFound();
        }
    }
}