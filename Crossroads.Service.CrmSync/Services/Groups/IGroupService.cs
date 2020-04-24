using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crossroads.Service.CrmSync.Services.Groups
{
    public interface IGroupService
    {
        Task<bool> CreateGroupParticipantsFromFormData();
    }
}
