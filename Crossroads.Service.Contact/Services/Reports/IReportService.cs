using Crossroads.Service.Contact.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crossroads.Service.Contact.Interfaces
{
    public interface IReportService
    {
        Task<List<AttendanceSummaryDto>> GetAttendanceSummary(int eventId);
    }
}
