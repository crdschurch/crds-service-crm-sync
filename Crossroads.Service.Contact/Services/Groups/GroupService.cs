using AutoMapper;
using Crossroads.Service.Contact.Interfaces;
using Crossroads.Service.Contact.Models;
using MinistryPlatform.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crossroads.Service.Contact.Services
{
    public class GroupService : IGroupService
    {
        IGroupRepository _groupRepository;
        IMapper _mapper;

        public GroupService(IGroupRepository groupRepository, IMapper mapper)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
        }

        public async Task<List<GroupDto>> GetGroups(int contactId, int eventId)
        {
            var mpGroups = await _groupRepository.GetEventGroupsByEventId(eventId);
            if (mpGroups != null)
            {
                return _mapper.Map<List<GroupDto>>(mpGroups).ToList();
            }
            return null;
        }
    }
}
