using AutoMapper;
using Crossroads.Service.Contact.Interfaces;
using Crossroads.Service.Contact.Models;
using MinistryPlatform.GroupParticipant;
using MinistryPlatform.Models;
using MinistryPlatform.Participants;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crossroads.Service.Contact.Services
{
    public class ParticipantService : IParticipantService
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly IGroupParticipantRepository _groupParticipantRepository;
        IMapper _mapper;

        private const int MpParticipationStatusAttended = 3;

        public ParticipantService(IParticipantRepository participantRepository, IGroupParticipantRepository groupParticipantRepository, IMapper mapper)
        {
            _participantRepository = participantRepository;
            _groupParticipantRepository = groupParticipantRepository;
            _mapper = mapper;
        }

        public async Task<int> UpdateParticipantAttending(GroupEventParticipantDto groupEventParticipant)
        {
            var mpGroupEventParticipant = _mapper.Map<MpGroupEventParticipant>(groupEventParticipant);
            mpGroupEventParticipant.ParticipantStatusId = MpParticipationStatusAttended;
            return await _participantRepository.UpdateParticipantAttending(mpGroupEventParticipant);
        }

        public async Task<int> UpdateParticipantNotAttending(int eventParticipantId)
        {
            return await _participantRepository.UpdateParticipantNotAttending(eventParticipantId);
        }

        public async Task<GroupParticipantDto> GetGroupParticipantUser(int eventId, int contactId)
        {
            var eventParticipant = await _participantRepository.GetGroupParticipant(eventId, contactId);
            var eventParticipantDto = _mapper.Map<GroupParticipantDto>(eventParticipant);
            return eventParticipantDto;
        }

        public async Task<List<GroupEventParticipantDto>> GetGroupEventParticipants(int groupId, int eventId)
        {
            var mpGroupEventParticipants = await _groupParticipantRepository.GetGroupEventParticipants(groupId, eventId);
            var groupEventParticipants = _mapper.Map<List<GroupEventParticipantDto>>(mpGroupEventParticipants).ToList();
            foreach (GroupEventParticipantDto groupEventParticipant in groupEventParticipants)
            {
                groupEventParticipant.EventId = eventId;
            }
            return groupEventParticipants;
        }
    }
}
