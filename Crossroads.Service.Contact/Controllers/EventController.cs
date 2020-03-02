using Crossroads.Service.Contact.Interfaces;
using Crossroads.Service.Contact.Models;
using Crossroads.Web.Auth.Controllers;
using Crossroads.Web.Auth.Models;
using Crossroads.Web.Common.Auth.Helpers;
using Crossroads.Web.Common.Security;
using Crossroads.Web.Common.Services;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System;
using System.Threading.Tasks;

namespace Crossroads.Service.Contact.Controllers
{
    [Route("api/[controller]")]
    [RequiresAuthorization]
    public class EventController : AuthBaseController
    {
        readonly IEventService _eventService;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public EventController(IAuthTokenExpiryService authTokenExpiryService,
            IAuthenticationRepository authenticationRepository,
            IEventService eventService)
            : base(authenticationRepository, authTokenExpiryService)
        {
            _eventService = eventService;
        }

        /// <summary>
        /// Gets a list of events for a given day for the logged in user.
        /// </summary>
        [HttpGet]
        [Route("date/{eventDate}")]
        [ProducesResponseType(typeof(EventDto), 200)]
        public async Task<IActionResult> GetEvents(DateTime eventDate)
        {
            var authDto = (AuthDTO)HttpContext.Items["authDto"];
            try
            {
                _logger.Info($"Getting events for Event ID: {eventDate} and Contact ID: {authDto.UserInfo.Mp.ContactId}");
                return Ok(await _eventService.GetEvents(authDto.UserInfo.Mp.ContactId, eventDate));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error getting events for Event ID: {eventDate} and Contact ID: {authDto.UserInfo.Mp.ContactId}");
                Console.WriteLine($"Error in GetEvents: {ex.Message}");
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Gets a spectific event for a given eventId.
        /// </summary>
        [HttpGet]
        [Route("{eventId}")]
        [ProducesResponseType(typeof(EventDto), 200)]
        public async Task<IActionResult> GetEvent(int eventId)
        {
            try
            {
                return Ok(await _eventService.GetEvent(eventId));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error in GetEvent: {ex}");
                Console.WriteLine($"Error in GetEvent: {ex.Message}");
                return StatusCode(500, ex);
            }
        }
    }
}
