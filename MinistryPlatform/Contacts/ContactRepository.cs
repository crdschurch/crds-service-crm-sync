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

namespace MinistryPlatform.Contacts
{
    public class ContactRepository : MinistryPlatformBase, IContactRepository
    {
        IAuthenticationRepository _authRepo;

        public ContactRepository(IMinistryPlatformRestRequestBuilderFactory builder,
            IApiUserRepository apiUserRepository,
            IConfigurationWrapper configurationWrapper,
            IMapper mapper,
            IAuthenticationRepository authenticationRepository) : base(builder, apiUserRepository, configurationWrapper,
            mapper)
        {
            _authRepo = authenticationRepository;
        }

        public async Task<MpContact> GetContactById(int contactId)
        {
            var token = ApiUserRepository.GetDefaultApiClientToken();
            var columns = new string[] {
                "Contacts.[Contact_ID]",
                "Contacts.[Nickname]",
                "Contacts.[Last_Name]",
                "Contacts.[Email_Address]"
            };
            var filter = $"Contacts.[Contact_ID] = {contactId}";
            var mpContacts = await MpRestBuilder.NewRequestBuilder()
                .WithAuthenticationToken(token)
                .WithSelectColumns(columns)
                .WithFilter(filter)
                .BuildAsync()
                .Search<MpContact>();

            if (!mpContacts.Any())
            {
                throw new Exception($"No contact found for contact: {contactId}");
            }

            return mpContacts.FirstOrDefault();
        }
    }
}
