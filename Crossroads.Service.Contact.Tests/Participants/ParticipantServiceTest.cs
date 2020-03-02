using AutoMapper;
using Crossroads.Service.Contact.Models;
using Crossroads.Service.Contact.Services;
using MinistryPlatform.GroupParticipant;
using MinistryPlatform.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Participants;
using Mock;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Crossroads.Service.Contact.Test.Participants
{
    public class ParticipantServiceTest
    {
        private readonly Mock<IParticipantRepository> _participantRepository;
        private readonly Mock<IGroupRepository> _groupRepository;
        private readonly Mock<IGroupParticipantRepository> _groupParticipantRepository;
        readonly Mock<IMapper> _mapper;
        private readonly ParticipantService _fixture;

        public ParticipantServiceTest()
        {
            _participantRepository = new Mock<IParticipantRepository>();
            _groupRepository = new Mock<IGroupRepository>();
            _groupParticipantRepository = new Mock<IGroupParticipantRepository>();
            _mapper = new Mock<IMapper>(MockBehavior.Strict);
            _fixture = new ParticipantService(_participantRepository.Object, _groupParticipantRepository.Object, _mapper.Object);
        }

        [Fact]
        public async void ShouldUpdateParticipantAttendanceChecked()
        {
            // Arrange
            _mapper.Setup(m => m.Map<MpGroupEventParticipant>(It.IsAny<GroupEventParticipantDto>()))
                .Returns(new MpGroupEventParticipant());
            _participantRepository.Setup(m => m.UpdateParticipantAttending(It.IsAny<MpGroupEventParticipant>()))
                .ReturnsAsync((int)MpGroupEventParticipantMock.GenerateMpGroupEventParticipants()[0].Single().EventParticipantId);

            // Act
            await _fixture.UpdateParticipantAttending(GroupEventParticipantMock.GenerateGroupEventParticipants().First().Single());

            // Assert
            _participantRepository.VerifyAll();
        }

        [Fact]
        public async void ShouldGetGroupAndEventParticipants()
        {
            // Arrange
            var groupId = 190586;
            var eventId = 4453723;

            _groupParticipantRepository.Setup(m => m.GetGroupEventParticipants(groupId, eventId))
                .ReturnsAsync(MpGroupEventParticipantMock.GenerateMpGroupEventParticipants()[0]);
            _mapper.Setup(m => m.Map<List<GroupEventParticipantDto>>(It.IsAny<List<MpGroupEventParticipant>>()))
                .Returns(new List<GroupEventParticipantDto>());

            // Act
            await _fixture.GetGroupEventParticipants(groupId, eventId);

            // Assert
            _participantRepository.VerifyAll();
        }
    }
}
