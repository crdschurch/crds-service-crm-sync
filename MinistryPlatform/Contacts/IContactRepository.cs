using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MinistryPlatform.Models;

namespace MinistryPlatform.Contacts
{
    public interface IContactRepository
    {
        Task<MpContact> GetContactById(int contactId);
    }
}
