using Crossroads.Service.Contact.Models;
using System;
using System.Collections.Generic;

namespace Mock
{
    public class EventMock
    {

        public static List<EventDto> CreateList() =>
            new List<EventDto>
            {
                new EventDto
                {
                    EventId = 123,
                    CongregationName = "Oakley",
                    EventStartDate = new DateTime(2018,10,31,6,30,00),
                    EventTitle = "Fathers AM"
                },
                new EventDto
                {
                    EventId = 124,
                    CongregationName = "Oakley",
                    EventStartDate = new DateTime(2019,3,17,18,45,00),
                    EventTitle = "Going Deeper"
                }
            };

        public static List<EventDto> CreateEmptyList() =>
            new List<EventDto> { };

        public static EventDto CreateEvent()
        {
            return new EventDto
            {
                EventId = 123,
                CongregationName = "Oakley",
                EventStartDate = new DateTime(2018, 10, 31, 6, 30, 00),
                EventTitle = "Fathers AM"
            };
        }
    }
}
