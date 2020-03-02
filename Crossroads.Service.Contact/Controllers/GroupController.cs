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
    public class GroupController : AuthBaseController
    {
        readonly IGroupService _groupService;
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public GroupController(IAuthTokenExpiryService authTokenExpiryService,
            IAuthenticationRepository authenticationRepository,
            IGroupService groupService)
            : base(authenticationRepository, authTokenExpiryService)
        {
            _groupService = groupService;
        }

        /// <summary>
        /// Gets a list of events for a given day for the logged in user.
        /// </summary>
        [HttpGet]
        [Route("eventId/{eventId}")]
        [ProducesResponseType(typeof(EventDto), 200)]
        public async Task<IActionResult> GetGroups(int eventId)
        {
            var authDto = (AuthDTO)HttpContext.Items["authDto"];
            try
            {
                _logger.Info($"Getting groups for Event ID: {eventId} and Contact ID: {authDto.UserInfo.Mp.ContactId}");
                return Ok(await _groupService.GetGroups(authDto.UserInfo.Mp.ContactId, eventId));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error getting groups for Event ID: {eventId} and Contact ID: {authDto.UserInfo.Mp.ContactId}");
                Console.WriteLine($"Error in GetGroups: {ex.Message}");
                return StatusCode(500, ex);
            }
        }
    }
}
