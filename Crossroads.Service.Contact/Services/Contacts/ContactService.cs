using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Crossroads.Service.Contact.Models;
using MinistryPlatform.Contacts;

namespace Crossroads.Service.Contact.Services.Contacts
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;

        public ContactService(IContactRepository contactRepository, IMapper mapper)
        {
            _contactRepository = contactRepository;
            _mapper = mapper;
        }

        public async Task<ContactDto> GetContactById(int contactId)
        {
            var mpContact = await _contactRepository.GetContactById(contactId);
            return _mapper.Map<ContactDto>(mpContact);
        }
    }
}
