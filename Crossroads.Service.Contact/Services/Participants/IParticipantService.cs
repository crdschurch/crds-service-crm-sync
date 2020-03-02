using Crossroads.Service.Contact.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crossroads.Service.Contact.Interfaces
{
    public interface IParticipantService
    {
        Task<int> UpdateParticipantAttending(GroupEventParticipantDto groupEventParticipant);
        Task<int> UpdateParticipantNotAttending(int eventParticipantId);
        Task<GroupParticipantDto> GetGroupParticipantUser(int groupId, int contactId);
        Task<List<GroupEventParticipantDto>> GetGroupEventParticipants(int groupId, int eventId);
    }
}
