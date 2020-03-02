using AutoMapper;
using Crossroads.Service.Contact.Interfaces;
using Crossroads.Service.Contact.Models;
using MinistryPlatform.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crossroads.Service.Contact.Services
{
    public class EventService : IEventService
    {
        IEventRepository _eventRepository;
        IMapper _mapper;

        public EventService(IEventRepository eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        public async Task<List<EventDto>> GetEvents(int contactId, DateTime eventDate)
        {
            var mpEvents = await _eventRepository.GetEvents(contactId, eventDate);
            if (mpEvents != null)
            {
                return _mapper.Map<List<EventDto>>(mpEvents).ToList();
            }
            return null;
        }

        public async Task<EventDto> GetEvent(int eventId)
        {
            var mpEvent = await _eventRepository.GetEvent(eventId);

            if (mpEvent != null)
            {
                return _mapper.Map<EventDto>(mpEvent);
            }

            return null;
        }
    }
}
