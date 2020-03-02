using AutoMapper;
using Crossroads.Service.Contact.Models;
using MinistryPlatform.Models;

public class MappingProfile : Profile
{
    private const int MpAttendedId = 3;

    public MappingProfile()
    {
        CreateMap<MpContact, ContactDto>();
    }
}
