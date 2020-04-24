using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MinistryPlatform.Models;
using Models.Models;

namespace MinistryPlatform.Groups
{
    public interface IGroupRepository
    {
        Task<List<MpGroupMembership>> GetGroupParticipation(DateTime startDate, DateTime endDate);
    }
}
