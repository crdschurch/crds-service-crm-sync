using Crossroads.Service.Contact.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crossroads.Service.Contact.Interfaces
{
    public interface IEventService
    {
        Task<List<EventDto>> GetEvents(int contactId, DateTime eventDate);
        Task<EventDto> GetEvent(int eventId);
    }
}
