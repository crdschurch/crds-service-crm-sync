using AutoMapper;
using Crossroads.Web.Common.Configuration;
using Crossroads.Web.Common.MinistryPlatform;
using Crossroads.Web.Common.Security;
using MinistryPlatform.Participants;
using Mock;
using Moq;
using Newtonsoft.Json.Linq;
using System.Linq;
using Xunit;

namespace MinistryPlatform.Test.Participants
{
    public class ParticipantRepositoryTest
    {
        readonly Mock<IApiUserRepository> _apiUserRepository;
        readonly Mock<IConfigurationWrapper> _configurationWrapper;
        readonly Mock<IMinistryPlatformRestRequestBuilder> _restRequest;
        readonly Mock<IMinistryPlatformRestRequestBuilderFactory> _restRequestBuilder;
        readonly Mock<IMinistryPlatformRestRequestAsync> _request;
        readonly Mock<IMapper> _mapper;
        readonly Mock<IAuthenticationRepository> _authenticationRepository;
        const string token = "token-345";
        const int contactId = 7344;
        const int householdId = 345345;

        private readonly ParticipantRepository _fixture;

        public ParticipantRepositoryTest()
        {
            _apiUserRepository = new Mock<IApiUserRepository>(MockBehavior.Strict);
            _restRequestBuilder = new Mock<IMinistryPlatformRestRequestBuilderFactory>(MockBehavior.Strict);
            _configurationWrapper = new Mock<IConfigurationWrapper>(MockBehavior.Strict);
            _restRequest = new Mock<IMinistryPlatformRestRequestBuilder>(MockBehavior.Strict);
            _mapper = new Mock<IMapper>(MockBehavior.Strict);
            _authenticationRepository = new Mock<IAuthenticationRepository>(MockBehavior.Strict);
            _request = new Mock<IMinistryPlatformRestRequestAsync>();

            _fixture = new ParticipantRepository(_restRequestBuilder.Object,
                _apiUserRepository.Object,
                _configurationWrapper.Object,
                _mapper.Object,
                _authenticationRepository.Object);
        }

        [Fact]
        public async void ShouldUpdateParticipantAttendance()
        {
            // Arrange
            _apiUserRepository.Setup(r => r.GetDefaultApiClientToken()).Returns(token);
            _restRequestBuilder.Setup(m => m.NewRequestBuilder()).Returns(_restRequest.Object);
            _restRequest.Setup(m => m.WithAuthenticationToken(token)).Returns(_restRequest.Object);
            _restRequest.Setup(m => m.BuildAsync()).Returns(_request.Object);

            var updateObject = new JObject();

            // Act
            await _fixture.UpdateParticipantAttending(MpGroupEventParticipantMock.GenerateMpGroupEventParticipants().First().Single());

            // Assert
            _restRequest.VerifyAll();
        }
    }
}
