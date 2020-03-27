using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Crossroads.Web.Common.Configuration;
using Crossroads.Web.Common.MinistryPlatform;
using Crossroads.Web.Common.Security;
using MinistryPlatform.Models;
using Models.Models;

namespace MinistryPlatform.Groups
{
    public class GroupRepository : MinistryPlatformBase, IGroupRepository
    {
        IAuthenticationRepository _authRepo;

        public GroupRepository(IMinistryPlatformRestRequestBuilderFactory builder,
            IApiUserRepository apiUserRepository,
            IConfigurationWrapper configurationWrapper,
            IMapper mapper,
            IAuthenticationRepository authenticationRepository) : base(builder, apiUserRepository, configurationWrapper,
            mapper)
        {
            _authRepo = authenticationRepository;
        }

        public async Task<List<MpGroupMembership>> GetGroupParticipation(DateTime startDate, DateTime endDate)
        {
            var token = await ApiUserRepository.GetDefaultApiClientTokenAsync(); // TODO: Update with api client

            var parameters = new Dictionary<string, object>
            {
                {"@StartDate", startDate},
                {"@EndDate", endDate},
                {"@GroupId", 198135}
            };

            var result = (await MpRestBuilder.NewRequestBuilder()
                .WithAuthenticationToken(token)
                .BuildAsync()
                .ExecuteStoredProc<MpGroupMembership>("api_crds_Get_GroupParticipants_For_Sync", parameters))[0].ToList();

            return result;
        }
    }
}
