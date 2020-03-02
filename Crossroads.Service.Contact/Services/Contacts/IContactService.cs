using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crossroads.Service.Contact.Models;

namespace Crossroads.Service.Contact.Services.Contacts
{
    public interface IContactService
    {
        Task<ContactDto> GetContactById(int contactId);
    }
}
