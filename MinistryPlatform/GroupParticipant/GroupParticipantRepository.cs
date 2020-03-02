using AutoMapper;
using Crossroads.Web.Common.Configuration;
using Crossroads.Web.Common.MinistryPlatform;
using MinistryPlatform.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MinistryPlatform.GroupParticipant
{
    public class GroupParticipantRepository : MinistryPlatformBase, IGroupParticipantRepository
    {
        public GroupParticipantRepository(IMinistryPlatformRestRequestBuilderFactory mpRestBuilder,
            IApiUserRepository apiUserRepository,
            IConfigurationWrapper configurationWrapper,
            IMapper mapper) : base(mpRestBuilder, apiUserRepository, configurationWrapper, mapper)
        {
        }

        public async Task<IList<MpGroupEventParticipant>> GetGroupEventParticipants(int groupId, int eventId)
        {
            var parms = new Dictionary<string, object>
            {
                {"@GroupId", groupId},
                {"@EventId", eventId}
            };

            var token = ApiUserRepository.GetDefaultApiClientToken();

            List<List<MpGroupEventParticipant>> data = await MpRestBuilder.NewRequestBuilder()
                .WithAuthenticationToken(token)
                .BuildAsync()
                .ExecuteStoredProc<MpGroupEventParticipant>("api_crds_Get_Group_Event_Participants", parms);

            var mpGroupEventParticipants = new List<MpGroupEventParticipant>();
            if (data[0].Count > 0)
            {
                mpGroupEventParticipants = data[0];
            }
            return mpGroupEventParticipants;
        }
    }
}
