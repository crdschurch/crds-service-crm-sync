using MinistryPlatform.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MinistryPlatform.GroupParticipant
{
    public interface IGroupParticipantRepository
    {
        Task<IList<MpGroupEventParticipant>> GetGroupEventParticipants(int groupId, int eventId);
    }
}
