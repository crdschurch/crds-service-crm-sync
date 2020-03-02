using AutoMapper;
using Crossroads.Web.Common.Configuration;
using Crossroads.Web.Common.MinistryPlatform;
using Crossroads.Web.Common.Security;
using MinistryPlatform.GroupParticipant;
using MinistryPlatform.Models;
using Mock;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace MinistryPlatform.Test.GroupParticipant
{
    public class GroupParticipantRepositoryTest
    {
        readonly Mock<IApiUserRepository> _apiUserRepository;
        readonly Mock<IConfigurationWrapper> _configurationWrapper;
        readonly Mock<IMinistryPlatformRestRequestBuilder> _restRequest;
        readonly Mock<IMinistryPlatformRestRequestBuilderFactory> _restRequestBuilder;
        readonly Mock<IMinistryPlatformRestRequest> _request;
        readonly Mock<IMapper> _mapper;
        readonly Mock<IAuthenticationRepository> _authenticationRepository;
        const string token = "token-345";

        private readonly GroupParticipantRepository _fixture;

        public GroupParticipantRepositoryTest()
        {
            _apiUserRepository = new Mock<IApiUserRepository>(MockBehavior.Strict);
            _restRequestBuilder = new Mock<IMinistryPlatformRestRequestBuilderFactory>(MockBehavior.Strict);
            _configurationWrapper = new Mock<IConfigurationWrapper>(MockBehavior.Strict);
            _restRequest = new Mock<IMinistryPlatformRestRequestBuilder>(MockBehavior.Strict);
            _mapper = new Mock<IMapper>(MockBehavior.Strict);
            _authenticationRepository = new Mock<IAuthenticationRepository>(MockBehavior.Strict);
            _request = new Mock<IMinistryPlatformRestRequest>();

            _fixture = new GroupParticipantRepository(_restRequestBuilder.Object,
                _apiUserRepository.Object,
                _configurationWrapper.Object,
                _mapper.Object);
        }

        [Fact]
        public void ShouldGetGroupEventParticipants()
        {
            int groupId = 176840;
            int eventId = 789799;

            _apiUserRepository.Setup(r => r.GetDefaultApiClientToken()).Returns(token);
            _restRequestBuilder.Setup(m => m.NewRequestBuilder()).Returns(_restRequest.Object);
            _restRequest.Setup(m => m.WithAuthenticationToken(token)).Returns(_restRequest.Object);
            _restRequest.Setup(m => m.Build()).Returns(_request.Object);
            _request.Setup(m => m.ExecuteStoredProc<MpGroupEventParticipant>("api_crds_Get_Group_Event_Participants", It.IsAny<Dictionary<string, object>>())).Returns(MpGroupEventParticipantMock.GenerateMpGroupEventParticipants());

            // Act
            var result = _fixture.GetGroupEventParticipants(groupId, eventId);

            // Assert
            Assert.NotNull(result);
        }
    }
}
