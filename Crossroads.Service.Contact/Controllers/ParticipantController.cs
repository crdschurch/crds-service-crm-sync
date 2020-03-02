using Crossroads.Service.Contact.Hubs;
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
    public class ParticipantController : AuthBaseController
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        readonly IParticipantService _participantService;

        public ParticipantController(IAuthTokenExpiryService authTokenExpiryService,
            IAuthenticationRepository authenticationRepository,
            IParticipantService participantService)
            : base(authenticationRepository, authTokenExpiryService)
        {
            _participantService = participantService;
        }

        /// <summary>
        ///  Get list of group and event participant object for an event
        /// </summary>
        [HttpGet]
        [Route("group/{groupId}/event/{eventId}")]
        [ProducesResponseType(typeof(GroupEventParticipantDto), 200)]
        public async Task<IActionResult> GetGroupEventParticipants(int groupId, int eventId)
        {
            try
            {
                return Ok(await _participantService.GetGroupEventParticipants(groupId, eventId));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error in GetGroupParticipants: {ex}");
                Console.WriteLine($"Error in GetGroupParticipants: {ex.Message}");
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Upsert Participant Attendance Status
        /// </summary>
        [HttpPut]
        [Route("eventParticipant/attending")]
        [ProducesResponseType(typeof(GroupEventParticipantDto), 200)]
        public async Task<IActionResult> UpdateParticipantAttending([FromBody] GroupEventParticipantDto groupEventParticipant)
        {
            try
            {
                var eventParticipantId = await _participantService.UpdateParticipantAttending(groupEventParticipant);
                if (eventParticipantId != 0)
                {
                    groupEventParticipant.EventParticipantId = eventParticipantId;
                    return Ok(groupEventParticipant);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error in UpdateParticipantAttendance: {ex}");
                Console.WriteLine($"Error in UpdateParticipantAttendance: {ex.Message}");
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Update Participant Not Attending
        /// </summary>
        [HttpPut]
        [Route("eventParticipant/notAttending")]
        [ProducesResponseType(typeof(GroupEventParticipantDto), 200)]
        public async Task<IActionResult> UpdateParticipantNotAttending([FromBody] GroupEventParticipantDto groupEventParticipant)
        {
            try
            {
                var eventParticipantId = await _participantService.UpdateParticipantNotAttending((int)groupEventParticipant.EventParticipantId);
                groupEventParticipant.EventParticipantId = eventParticipantId;
                return Ok(groupEventParticipant);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error in UpdateParticipantNotAttending: {ex}");
                Console.WriteLine($"Error in UpdateParticipantNotAttending: {ex.Message}");
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Gets the currently logged in Group Participant object for a given event id.
        /// </summary>
        [HttpGet]
        [Route("event/{eventId}/user")]
        [ProducesResponseType(typeof(GroupParticipantDto), 200)]
        public async Task<IActionResult> GetGroupParticipantUser(int eventId)
        {
            var authDto = (AuthDTO)HttpContext.Items["authDto"];
            try
            {
                var contactId = authDto.UserInfo.Mp.ContactId;
                return Ok(await _participantService.GetGroupParticipantUser(eventId, contactId));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error in GetGroupParticipantUser: {ex}");
                Console.WriteLine($"Error in GetGroupParticipantUser: {ex.Message}");
                return StatusCode(500, ex);
            }
        }
    }
}
