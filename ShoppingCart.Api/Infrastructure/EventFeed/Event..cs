﻿using System;

namespace ShoppingCart.Api.Infrastructure.EventFeed
{
    public class Event 
    {
        public long SequenceNumber { get; }
        public DateTimeOffset OccuredAt { get; }
        public string Name { get; }
        public object Content { get; }

        public Event(
          long sequenceNumber,
          DateTimeOffset occuredAt,
          string name,
          object content)
        {
            this.SequenceNumber = sequenceNumber;
            this.OccuredAt = occuredAt;
            this.Name = name;
            this.Content = content;
        }
    }
}
