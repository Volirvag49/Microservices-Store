using System.Collections.Generic;

namespace ShoppingCart.Api.Infrastructure.EventFeed.Interfaces
{
    public interface IEventStore
    {
        IEnumerable<Event> GetEvents(long firstEventSequenceNumber, long lastEventSequenceNumber);
        void Raise(string eventName, object content);
    }
}
