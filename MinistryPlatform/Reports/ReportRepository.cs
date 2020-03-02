using AutoMapper;
using Crossroads.Web.Common.Configuration;
using Crossroads.Web.Common.MinistryPlatform;
using Crossroads.Web.Common.Security;
using MinistryPlatform.Interfaces;
using MinistryPlatform.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MinistryPlatform.Repositories
{
    public class ReportRepository : MinistryPlatformBase, IReportRepository
    {
        public ReportRepository(IMinistryPlatformRestRequestBuilderFactory builder,
            IApiUserRepository apiUserRepository,
            IConfigurationWrapper configurationWrapper,
            IMapper mapper,
            IAuthenticationRepository authenticationRepository) : base(builder, apiUserRepository, configurationWrapper, mapper)
        {

        }

        public async Task<List<MpAttendanceSummary>> GetAttendanceSummary(int eventId)
        {
            var parms = new Dictionary<string, object>
            {
                {"@EventId", eventId}
            };

            var token = ApiUserRepository.GetDefaultApiClientToken();

            var data = await MpRestBuilder.NewRequestBuilder()
                .WithAuthenticationToken(token)
                .BuildAsync()
                .ExecuteStoredProc<MpAttendanceSummary>("api_crds_Get_Attendance_Summary", parms);

            var mpAttendanceSummary = new List<MpAttendanceSummary>();

            foreach(var summaryRecord in data[0])
            {
                mpAttendanceSummary.Add(summaryRecord);
            }

            return mpAttendanceSummary;
        }
    }
}
