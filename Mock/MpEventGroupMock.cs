using System;
using System.Collections.Generic;
using System.Text;
using MinistryPlatform.Models;

namespace Mock
{
    public class MpEventGroupMock
    {
        public static List<MpEventGroup> GenerateEventGroups()
        {
            var mpEventGroups = new List<MpEventGroup>();

            mpEventGroups.Add(new MpEventGroup
            {
                GroupId = 1235678,
                EventId = 7685234
            });

            return mpEventGroups;
        }
    }
}
