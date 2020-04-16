using System.Threading.Tasks;
using Crossroads.Service.CrmSync.Models;

namespace Crossroads.Service.CrmSync.Services.Contacts
{
    public interface IContactService
    {
        Task<bool> SyncGroupParticipantData();
    }
}
