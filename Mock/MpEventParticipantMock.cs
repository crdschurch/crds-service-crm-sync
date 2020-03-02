using MinistryPlatform.Models;
using System.Collections.Generic;

namespace Mock
{
    public class MpEventParticipantMock
    {
        public static List<MpEventParticipant> GenerateMpEventParticipants()
        {
            var mpEventParticipants = new List<MpEventParticipant>();

            mpEventParticipants.Add(new MpEventParticipant
            {
                ParticipantId = 7123456,
                ParticipantStatusId = 3,
                LastName = "Builder",
                Nickname = "Bob"
            });

            return mpEventParticipants;
        }
    }
}
