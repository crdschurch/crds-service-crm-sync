using AutoMapper;
using Crossroads.Web.Common.Configuration;
using Crossroads.Web.Common.MinistryPlatform;
using Crossroads.Web.Common.Security;
using MinistryPlatform.Models;
using MinistryPlatform.Repositories;
using Mock;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MinistryPlatform.Test.Contacts
{
    public class EventRepositoryTest
    {
        readonly EventRepository _fixture;
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

        public EventRepositoryTest()
        {
            _apiUserRepository = new Mock<IApiUserRepository>(MockBehavior.Strict);
            _restRequestBuilder = new Mock<IMinistryPlatformRestRequestBuilderFactory>(MockBehavior.Strict);
            _configurationWrapper = new Mock<IConfigurationWrapper>(MockBehavior.Strict);
            _restRequest = new Mock<IMinistryPlatformRestRequestBuilder>(MockBehavior.Strict);
            _mapper = new Mock<IMapper>(MockBehavior.Strict);
            _authenticationRepository = new Mock<IAuthenticationRepository>(MockBehavior.Strict);
            _request = new Mock<IMinistryPlatformRestRequestAsync>();
            _fixture = new EventRepository(_restRequestBuilder.Object,
                                             _apiUserRepository.Object,
                                             _configurationWrapper.Object,
                                             _mapper.Object,
                                             _authenticationRepository.Object);
        }

        [Fact]
        public async void GetOneEvent()
        {
            //Arrange
            _apiUserRepository.Setup(r => r.GetDefaultApiClientToken()).Returns(token);
            _restRequestBuilder.Setup(m => m.NewRequestBuilder()).Returns(_restRequest.Object);
            _restRequest.Setup(m => m.WithAuthenticationToken(token)).Returns(_restRequest.Object);
            _restRequest.Setup(m => m.BuildAsync()).Returns(_request.Object);
            _request.Setup(m => m.Search<MpEvent>()).Returns(Task.FromResult(MpEventMock.CreateList()));

            var returnList = new List<List<JObject>>();
            var objectList = new List<JObject>
            {
                new JObject()
            };
            returnList.Add(objectList);

            _request.Setup(m => m.ExecuteStoredProc<JObject>("api_crds_Get_Event_Checkin_Events", It.IsAny<Dictionary<string, object>>())).Returns(Task.FromResult(returnList));

            //Act
            var response = await _fixture.GetEvents(1, new DateTime(2018,10,31));
            
            //Assert
            Assert.Single(response);
        }

        [Fact]
        public async void GetEventEmpty()
        {
            //Arrange
            _apiUserRepository.Setup(r => r.GetDefaultApiClientToken()).Returns(token);
            _restRequestBuilder.Setup(m => m.NewRequestBuilder()).Returns(_restRequest.Object);
            _restRequest.Setup(m => m.WithAuthenticationToken(token)).Returns(_restRequest.Object);
            _restRequest.Setup(m => m.BuildAsync()).Returns(_request.Object);
            _request.Setup(m => m.Search<MpEvent>()).Returns(Task.FromResult(MpEventMock.CreateList()));

            var returnList = new List<List<JObject>>();
            var objectList = new List<JObject> { };
            returnList.Add(objectList);

            _request.Setup(m => m.ExecuteStoredProc<JObject>("api_crds_Get_Event_Checkin_Events", It.IsAny<Dictionary<string, object>>())).Returns(Task.FromResult(returnList));

            //Act & Assert
            Assert.Empty(await _fixture.GetEvents(1, new DateTime(2018, 10, 31)));
        }

        [Fact]
        public async void GetSpecificEvent()
        {
            //Arrange
            var eventId = 123;
            var selectColumns = new string[] {
                "Events.[Event_ID]",
                "Events.[Event_Title]",
                "Congregation_ID_Table.[Congregation_Name]",
                "Events.[Event_Start_Date]",
                "Events.[Event_End_Date]"
            };
            var filter = "Events.[Event_ID] = 123";

            _apiUserRepository.Setup(r => r.GetDefaultApiClientToken()).Returns(token);
            _restRequestBuilder.Setup(m => m.NewRequestBuilder()).Returns(_restRequest.Object);
            _restRequest.Setup(m => m.WithAuthenticationToken(token)).Returns(_restRequest.Object);
            _restRequest.Setup(m => m.WithSelectColumns(selectColumns)).Returns(_restRequest.Object);
            _restRequest.Setup(m => m.WithFilter(filter)).Returns(_restRequest.Object);
            _restRequest.Setup(m => m.BuildAsync()).Returns(_request.Object);

            _request.Setup(m => m.Search<MpEvent>()).Returns(Task.FromResult(MpEventMock.CreateList()));

            //Act
            var responseEvent = await _fixture.GetEvent(eventId);

            //Assert
            Assert.Equal(eventId, responseEvent.EventId);
        }
    }
}
