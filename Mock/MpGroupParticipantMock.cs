using MinistryPlatform.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mock
{
    public class MpGroupParticipantMock
    {
        public static List<MpGroupParticipant> GenerateGroupParticipants()
        {
            var mpGroupParticipants = new List<MpGroupParticipant>();

            mpGroupParticipants.Add(new MpGroupParticipant
            {
                ContactId = 7123456
            });

            return mpGroupParticipants;
        }
    }
}
