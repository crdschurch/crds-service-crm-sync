using System;
using System.Threading.Tasks;
using AutoMapper;
using Crossroads.Service.CrmSync.Models;
using ExternalSync.Hubspot;
using MinistryPlatform.Contacts;
using MinistryPlatform.Groups;

namespace Crossroads.Service.CrmSync.Services.Contacts
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IHubspotClient _hubspotClient;
        private readonly IMapper _mapper;

        public ContactService(IContactRepository contactRepository, IGroupRepository groupRepository, 
            IHubspotClient hubspotClient, IMapper mapper)
        {
            _contactRepository = contactRepository;
            _groupRepository = groupRepository;
            _hubspotClient = hubspotClient;
            _mapper = mapper;
        }

        public async Task<ContactDto> GetContactById(int contactId)
        {
            var mpContact = await _contactRepository.GetContactById(contactId);
            return _mapper.Map<ContactDto>(mpContact);
        }

        public async Task<bool> SyncGroupParticipantData()
        {
            var data = await _groupRepository.GetGroupParticipation(DateTime.Now.AddDays(-1), DateTime.Now);
            var result = await _hubspotClient.SyncGroupParticipationData(data);
            return result;
        }
    }
}
