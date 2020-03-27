using AutoMapper;
using Crossroads.Service.CrmSync.Models;
using MinistryPlatform.Models;

namespace Crossroads.Service.CrmSync
{
    public class MappingProfile : Profile
    {
        private const int MpAttendedId = 3;

        public MappingProfile()
        {
            CreateMap<MpContact, ContactDto>();
        }
    }
}
