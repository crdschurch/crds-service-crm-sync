using AutoMapper;
using Crossroads.Web.Common.Configuration;
using Crossroads.Web.Common.MinistryPlatform;
using Crossroads.Web.Common.Security;
using MinistryPlatform.Interfaces;
using MinistryPlatform.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinistryPlatform.Repositories
{
    public class EventRepository : MinistryPlatformBase, IEventRepository
    {
        IAuthenticationRepository _authRepo;

        public EventRepository(IMinistryPlatformRestRequestBuilderFactory builder,
                               IApiUserRepository apiUserRepository,
                               IConfigurationWrapper configurationWrapper,
                               IMapper mapper,
                               IAuthenticationRepository authenticationRepository) : base(builder, apiUserRepository, configurationWrapper, mapper)
        {
            _authRepo = authenticationRepository;
        }

        public async Task<List<MpEvent>> GetEvents(int contactId, DateTime eventDate)
        {
            var parms = new Dictionary<string, object>
            {
                {"@ContactId", contactId},
                {"@EventDate", eventDate}
            };

            var token = ApiUserRepository.GetDefaultApiClientToken();

            var data = await MpRestBuilder.NewRequestBuilder()
                .WithAuthenticationToken(token)
                .BuildAsync()
                .ExecuteStoredProc<JObject>("api_crds_Get_Event_Checkin_Events", parms);

            var events = new List<MpEvent>();

            if (data[0].Count > 0)
            {
                events = data[0].Select(r => r.ToObject<MpEvent>()).ToList();
            }

            return events;
        }

        public async Task<MpEvent> GetEvent(int eventId)
        {
            var token = ApiUserRepository.GetDefaultApiClientToken();
            var columns = new string[] {
                "Events.[Event_ID]",
                "Events.[Event_Title]",
                "Congregation_ID_Table.[Congregation_Name]",
                "Events.[Event_Start_Date]",
                "Events.[Event_End_Date]"
            };
            var filter = $"Events.[Event_ID] = {eventId}";
            var mpEvents = await MpRestBuilder.NewRequestBuilder()
                .WithAuthenticationToken(token)
                .WithSelectColumns(columns)
                .WithFilter(filter)
                .BuildAsync()
                .Search<MpEvent>();

            if (!mpEvents.Any())
            {
                throw new Exception($"No event found for event: {eventId}");
            }

            return mpEvents.FirstOrDefault();
        }
    }
}
