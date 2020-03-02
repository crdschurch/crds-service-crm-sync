using MinistryPlatform.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MinistryPlatform.Interfaces
{
    public interface IEventRepository
    {
        Task<List<MpEvent>> GetEvents(int contactId, DateTime eventDate);
        Task<MpEvent> GetEvent(int eventId);
    }
}
