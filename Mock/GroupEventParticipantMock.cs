using Crossroads.Service.Contact.Models;
using MinistryPlatform.Models;
using System.Collections.Generic;

namespace Mock
{
    public static class GroupEventParticipantMock
    {
        public static List<List<GroupEventParticipantDto>> GenerateGroupEventParticipants()
        {
            var groupEventParticipants = new List<GroupEventParticipantDto>();

            groupEventParticipants.Add(new GroupEventParticipantDto
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
                Attending = true
            });

            var returnData = new List<List<GroupEventParticipantDto>>();
            returnData.Add(groupEventParticipants);

            return returnData;
        }
    }
}
