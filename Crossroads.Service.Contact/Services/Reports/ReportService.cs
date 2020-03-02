using AutoMapper;
using Crossroads.Service.Contact.Interfaces;
using Crossroads.Service.Contact.Models;
using MinistryPlatform.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crossroads.Service.Contact.Services
{
    public class ReportService : IReportService
    {
        IReportRepository _reportRepository;
        IMapper _mapper;

        public ReportService(IReportRepository reportRepository, IMapper mapper)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
        }

        public async Task<List<AttendanceSummaryDto>> GetAttendanceSummary(int eventId)
        {
            var mpAttendanceSummary = await _reportRepository.GetAttendanceSummary(eventId);
            if (mpAttendanceSummary != null)
            {
                return _mapper.Map<List<AttendanceSummaryDto>>(mpAttendanceSummary).ToList();
            }
            return null;
        }
    }
}
