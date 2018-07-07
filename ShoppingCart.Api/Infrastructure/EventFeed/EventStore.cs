 using ShoppingCart.Api.Infrastructure.EventFeed.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ShoppingCart.Api.Infrastructure.EventFeed
{
    public class EventStore : IEventStore
    {
        private static long currentSequenceNumber = 0;
        private static readonly IList<Event> database = new List<Event>();

        public IEnumerable<Event> GetEvents(long firstEventSequenceNumber, long lastEventSequenceNumber)
        {
            var events = database
              .Where(e => e.SequenceNumber >= firstEventSequenceNumber && e.SequenceNumber <= lastEventSequenceNumber)
              .OrderBy(e => e.SequenceNumber);

            return events;
        }

        public void Raise(string eventName, object content)
        {
            var seqNumber = Interlocked.Increment(ref currentSequenceNumber);
            database.Add(
              new Event(seqNumber,DateTimeOffset.UtcNow, eventName, content));
        }
    }
}
