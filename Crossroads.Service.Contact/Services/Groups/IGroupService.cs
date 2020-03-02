using Crossroads.Service.Contact.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crossroads.Service.Contact.Interfaces
{
    public interface IGroupService
    {
        Task<List<GroupDto>> GetGroups(int contactId, int eventId);
    }
}
