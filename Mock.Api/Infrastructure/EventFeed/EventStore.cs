using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Mock.Api.Infrastructure.EventFeed
{
    public class EventStore 
    {
        private static long currentSequenceNumber = 0;
        private static readonly IList<Event> database = new List<Event>();

        public IEnumerable<Event> GetEvents(long firstEventSequenceNumber, long lastEventSequenceNumber)
        {
            var events = database
              .Where(e => e.SequenceNumber >= firstEventSequenceNumber && e.SequenceNumber <= lastEventSequenceNumber)
              .OrderBy(e => e.SequenceNumber).ToList();

            return events;
        }

        public void Raise(string eventName, object content)
        {
            var seqNumber = Interlocked.Increment(ref currentSequenceNumber);
            database.Add(
              new Event(seqNumber, DateTimeOffset.UtcNow, eventName, content));
        }
    }
}
