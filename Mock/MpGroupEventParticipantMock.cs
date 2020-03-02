using MinistryPlatform.Models;
using System.Collections.Generic;

namespace Mock
{
    public static class MpGroupEventParticipantMock
    {
        public static List<List<MpGroupEventParticipant>> GenerateMpGroupEventParticipants()
        {
            var mpGroupEventParticipants = new List<MpGroupEventParticipant>();

            mpGroupEventParticipants.Add(new MpGroupEventParticipant
            {
                ParticipantId = 7667621,
                ContactId = 7785751,
                GroupId = 176840,
                EventId = 1234,
                GroupParticipantId = 14604420,
                EventParticipantId = 8500000,
                Nickname = "Bobby",
                LastName = "Builder",
                IsLeader = true,
                IsMember = false,
                ParticipantStatusId = 1,
                DomainId = 1,
                GroupRoleId = 22
            });

            var returnData = new List<List<MpGroupEventParticipant>>();
            returnData.Add(mpGroupEventParticipants);

            return returnData;
        }
    }
}
