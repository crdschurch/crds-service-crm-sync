using MinistryPlatform.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MinistryPlatform.Interfaces
{
    public interface IReportRepository
    {
        Task<List<MpAttendanceSummary>> GetAttendanceSummary(int eventId);
    }
}
