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
        private readonly IGroupRepository _groupRepository;
        private readonly IHubspotClient _hubspotClient;

        public ContactService(
            IGroupRepository groupRepository, 
            IHubspotClient hubspotClient
            )
        {
            _groupRepository = groupRepository;
            _hubspotClient = hubspotClient;
        }

        public async Task<bool> SyncGroupParticipantData()
        {
            var data = await _groupRepository.GetGroupParticipation(DateTime.Now.AddDays(-180), DateTime.Now);
            var result = await _hubspotClient.SyncGroupParticipationData(data);
            return result;
        }
    }
}
