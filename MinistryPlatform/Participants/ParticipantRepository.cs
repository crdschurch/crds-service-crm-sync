using AutoMapper;
using Crossroads.Web.Common.Configuration;
using Crossroads.Web.Common.MinistryPlatform;
using Crossroads.Web.Common.Security;
using MinistryPlatform.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinistryPlatform.Participants
{
    public class ParticipantRepository : MinistryPlatformBase, IParticipantRepository
    {
        const int GroupRoleMemberId = 16;
        const int GroupRoleLeaderId = 22;

        public ParticipantRepository(IMinistryPlatformRestRequestBuilderFactory builder,
            IApiUserRepository apiUserRepository,
            IConfigurationWrapper configurationWrapper,
            IMapper mapper,
            IAuthenticationRepository authenticationRepository) : base(builder, apiUserRepository, configurationWrapper, mapper)
        {

        }

        public async Task<List<MpGroupParticipant>> GetGroupParticipantsByGroupIds(List<int> groupsIds)
        {
            var token = ApiUserRepository.GetDefaultApiClientToken();
            var columns = new string[] {
                "Group_Participants.[Group_Participant_ID]",
                "Group_ID_Table.[Group_ID]",
                "Group_Participants.[Participant_ID]",
                "Participant_ID_Table_Contact_ID_Table.[Contact_ID]",
                "Participant_ID_Table_Contact_ID_Table.[Nickname]",
                "Participant_ID_Table_Contact_ID_Table.[Last_Name]"
            };
            var filter = $"Group_Participants.Group_ID IN ({string.Join(",", groupsIds)})";
            var mpGroupParticipants = await MpRestBuilder.NewRequestBuilder()
                .WithAuthenticationToken(token)
                .WithSelectColumns(columns)
                .WithFilter(filter)
                .BuildAsync()
                .Search<MpGroupParticipant>();

            return mpGroupParticipants;
        }

        public async Task<int> UpdateParticipantAttending(MpGroupEventParticipant groupEventParticipant)
        {
            var token = ApiUserRepository.GetDefaultApiClientToken();

            if (groupEventParticipant.EventParticipantId > 0)
            {
                var updateObject = new JObject
                {
                    { "Event_Participant_ID", groupEventParticipant.EventParticipantId},
                    { "Participation_Status_ID", groupEventParticipant.ParticipantStatusId},
                    { "Group_Participant_ID", groupEventParticipant.GroupParticipantId },
                    { "Group_ID", groupEventParticipant.GroupId }
                };

                var response = await MpRestBuilder.NewRequestBuilder()
                    .WithAuthenticationToken(token)
                    .BuildAsync()
                    .Update(updateObject, "Event_Participants");

                return 0;
            }
            else
            {
                int groupRoleId = groupEventParticipant.IsLeader == true ? GroupRoleLeaderId : GroupRoleMemberId;

                var insertObject = new MpGroupEventParticipant
                {
                    EventParticipantId = groupEventParticipant.EventParticipantId ,
                    EventId = groupEventParticipant.EventId,
                    ParticipantId = groupEventParticipant.ParticipantId,
                    ParticipantStatusId = groupEventParticipant.ParticipantStatusId,
                    DomainId = 1 ,
                    GroupParticipantId = groupEventParticipant.GroupParticipantId,
                    GroupId = groupEventParticipant.GroupId,
                    GroupRoleId = groupRoleId
                };

                var response = await MpRestBuilder.NewRequestBuilder()
                    .WithAuthenticationToken(token)
                    .BuildAsync()
                    .Create(insertObject, "Event_Participants");

                return (int)response.EventParticipantId;
            }
        }

        public async Task<int> UpdateParticipantNotAttending(int eventParticipantId)
        {
            var token = ApiUserRepository.GetDefaultApiClientToken();

            var parms = new Dictionary<string, object>
            {
                {"@EventParticipantId", eventParticipantId}
            };

            var data = await MpRestBuilder.NewRequestBuilder()
                .WithAuthenticationToken(token)
                .BuildAsync()
                .ExecuteStoredProc<JObject>("api_crds_Revert_Participant_Attendance_Status", parms);

            return (int)data[0].First().GetValue("ReturnId");
        }

        public async Task<MpGroupParticipant> GetGroupParticipant(int eventId, int contactId)
        {
            var parms = new Dictionary<string, object>
            {
                {"@ContactId", contactId},
                {"@EventId", eventId}
            };

            var token = ApiUserRepository.GetDefaultApiClientToken();

            var data = await MpRestBuilder.NewRequestBuilder()
                .WithAuthenticationToken(token)
                .BuildAsync()
                .ExecuteStoredProc<MpGroupParticipant>("api_crds_Get_Group_Roles_By_Event", parms);

            var mpGroupParticipant = new MpGroupParticipant();

            if (data[0].Count > 0)
            {
                mpGroupParticipant = data[0].FirstOrDefault();
            }

            return mpGroupParticipant;
        }
    }
}
