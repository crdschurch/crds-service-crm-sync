using AutoMapper;
using Crossroads.Service.Contact.Models;
using MinistryPlatform.Models;

public class MappingProfile : Profile
{
    private const int MpAttendedId = 3;

    public MappingProfile()
    {
        CreateMap<MpContact, ContactDto>();
        CreateMap<MpEvent, EventDto>();
        CreateMap<MpEventParticipant, EventParticipantDto>()
            .ForMember(dest => dest.Attending, opt => opt.MapFrom(r => r.ParticipantStatusId == MpAttendedId ? true : false));
        CreateMap<MpGroupParticipant, GroupParticipantDto>();
        CreateMap<MpGroupEventParticipant, GroupEventParticipantDto>()
            .ForMember(dest => dest.Attending, opt => opt.MapFrom(r => r.ParticipantStatusId == MpAttendedId ? true : false));
        CreateMap<GroupEventParticipantDto, MpGroupEventParticipant>();
        CreateMap<MpAttendanceSummary, AttendanceSummaryDto>();
        CreateMap<MpEventGroup, GroupDto>();
    }
}
