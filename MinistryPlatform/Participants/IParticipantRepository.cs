using MinistryPlatform.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MinistryPlatform.Participants
{
    public interface IParticipantRepository
    {
        Task<List<MpGroupParticipant>> GetGroupParticipantsByGroupIds(List<int> groupsIds);
        Task<int> UpdateParticipantAttending(MpGroupEventParticipant mpGroupEventParticipant);
        Task<int> UpdateParticipantNotAttending(int eventParticipantId);
        Task<MpGroupParticipant> GetGroupParticipant(int groupId, int contactId);
    }
}
