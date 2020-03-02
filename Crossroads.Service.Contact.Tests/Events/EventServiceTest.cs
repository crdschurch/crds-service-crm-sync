using AutoMapper;
using Crossroads.Service.Contact.Models;
using Crossroads.Service.Contact.Services;
using MinistryPlatform.Interfaces;
using MinistryPlatform.Models;
using Mock;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Crossroads.Service.Contact.Test.Events
{
    public class EventServiceTest
    {
        readonly Mock<IEventRepository> _eventRepository;
        readonly Mock<IMapper> _mapper;
        readonly EventService _eventService;

        public EventServiceTest()
        {
            _eventRepository = new Mock<IEventRepository>(MockBehavior.Strict);
            _mapper = new Mock<IMapper>(MockBehavior.Strict);
            _eventService = new EventService(_eventRepository.Object, _mapper.Object);
        }


        [Fact]
        public async void ShouldGetMultipleEvents()
        {
            //Arrange
            _eventRepository.Setup(m => m.GetEvents(1, new DateTime(2018,10,31))).ReturnsAsync(MpEventMock.CreateList());
            _mapper.Setup(m => m.Map<List<EventDto>>(It.IsAny<List<MpEvent>>())).Returns(EventMock.CreateList());
            //Act
            var result = await _eventService.GetEvents(1, new DateTime(2018,10,31));
            //Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async void ShouldGetZeroEvents()
        {
            //Arrange
            _eventRepository.Setup(m => m.GetEvents(1, new DateTime(2018, 10, 31))).ReturnsAsync(MpEventMock.CreateEmptyList);
            _mapper.Setup(m => m.Map<List<EventDto>>(It.IsAny<List<MpEvent>>())).Returns(EventMock.CreateEmptyList());
            //Act
            var result = await _eventService.GetEvents(1, new DateTime(2018, 10, 31));
            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public async void GetEvent()
        {
            //Arrange
            _eventRepository.Setup(m => m.GetEvent(123)).ReturnsAsync(MpEventMock.CreateEvent());
            _mapper.Setup(m => m.Map<EventDto>(It.IsAny<MpEvent>())).Returns(EventMock.CreateEvent());
            //Act
            var result = await _eventService.GetEvent(123);
            //Assert
            Assert.NotNull(result);
        }
    }
}
