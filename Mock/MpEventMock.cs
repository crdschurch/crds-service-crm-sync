using MinistryPlatform.Models;
using System;
using System.Collections.Generic;

namespace Mock
{
    public class MpEventMock
    {

        public static List<MpEvent> CreateList() =>
            new List<MpEvent>
            {
                new MpEvent
                {
                    EventId = 123,
                    EventTitle = "Fathers AM",
                    CongregationName = "Oakley",
                    EventStartDate = new DateTime(2018,10,31,6,30,00),
                    EventEndDate = new DateTime(2018,10,31,8,00,00),
                    GroupId = 456,
                    GroupName = "Fathers AM"
                },
                new MpEvent
                {
                    EventId = 124,
                    EventTitle = "Going Deeper",
                    CongregationName = "Oakley",
                    EventStartDate = new DateTime(2019,3,17,18,45,00),
                    EventEndDate = new DateTime(2018,3,17,20,00,00),
                    GroupId = 457,
                    GroupName = "Going Deeper"
                }
            };

        public static List<MpEvent> CreateEmptyList() =>
            new List<MpEvent> { };

        public static MpEvent CreateEvent() =>
            new MpEvent
            {
                EventId = 123,
                EventTitle = "Fathers AM",
                CongregationName = "Oakley",
                EventStartDate = new DateTime(2018, 10, 31, 6, 30, 00),
                EventEndDate = new DateTime(2018, 10, 31, 8, 00, 00),
                GroupId = 456,
                GroupName = "Fathers AM"
            };
    }
}
