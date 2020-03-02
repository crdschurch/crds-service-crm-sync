using AutoMapper;
using Crossroads.Web.Common.Configuration;
using Crossroads.Web.Common.MinistryPlatform;
using Crossroads.Web.Common.Security;
using MinistryPlatform.Interfaces;
using MinistryPlatform.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinistryPlatform.Repositories
{
    public class GroupRepository : MinistryPlatformBase, IGroupRepository
    {
        public GroupRepository(IMinistryPlatformRestRequestBuilderFactory builder,
            IApiUserRepository apiUserRepository,
            IConfigurationWrapper configurationWrapper,
            IMapper mapper,
            IAuthenticationRepository authenticationRepository) : base(builder, apiUserRepository, configurationWrapper,
            mapper)
        {
            
        }

        public async Task<List<MpEventGroup>> GetEventGroupsByEventId(int eventId)//, List<int> groupTypeIds)
        {
            var parms = new Dictionary<string, object>
            {
                {"@EventId", eventId}
            };

            var token = ApiUserRepository.GetDefaultApiClientToken();

            var data = await MpRestBuilder.NewRequestBuilder()
                .WithAuthenticationToken(token)
                .BuildAsync()
                .ExecuteStoredProc<JObject>("api_crds_Get_Event_Groups", parms);

            var events = new List<MpEventGroup>();

            if (data[0].Count > 0)
            {
                events = data[0].Select(r => r.ToObject<MpEventGroup>()).ToList();
            }

            return events;
        }
    }
}
