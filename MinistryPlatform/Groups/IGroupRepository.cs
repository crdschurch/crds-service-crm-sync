using MinistryPlatform.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MinistryPlatform.Interfaces
{
    public interface IGroupRepository
    {
        Task<List<MpEventGroup>> GetEventGroupsByEventId(int eventId);//, List<int> groupTypes);
    }
}
