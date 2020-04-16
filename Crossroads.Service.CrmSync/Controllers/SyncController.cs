using System;
using System.Threading.Tasks;
using Crossroads.Service.CrmSync.Services.Contacts;
using Crossroads.Service.CrmSync.Services.Groups;
using Crossroads.Web.Auth.Controllers;
using Crossroads.Web.Common.Auth.Helpers;
using Crossroads.Web.Common.Security;
using Crossroads.Web.Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace Crossroads.Service.CrmSync.Controllers
{
    [Route("api/[controller]")]
    [RequiresAuthorization]
    public class SyncController : AuthBaseController
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IContactService _contactService;

        public SyncController(
            IAuthTokenExpiryService authTokenExpiryService,
            IAuthenticationRepository authenticationRepository,
            IContactService contactService
            )
            : base(authenticationRepository, authTokenExpiryService)
        {
            _contactService = contactService;
        }

        /// <summary>
        /// Simple Hello World route for testing.
        /// </summary>
        [HttpGet]
        [Route("hello")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), 200)]
        public IActionResult HelloWorld()
        {
            try
            {
                return Ok("Hello world!");
            }
            catch (Exception ex)
            {
                _logger.Error($"Error in HellowWorld: {ex}");
                Console.WriteLine($"Error in HelloWorld: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Secure Hello World route for testing.
        /// </summary>
        [HttpGet("secure-hello")]
        [ProducesResponseType(typeof(string), 200)]
        public IActionResult SecureHelloWorld()
        {
            try
            {
                return Ok("Hello secure world!");
            }
            catch (Exception ex)
            {
                _logger.Error($"Error in SecureHelloWorld: {ex}");
                Console.WriteLine($"Error in SecureHelloWorld: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Sync predefined groups to HubSpot
        /// </summary>
        [HttpGet("/groups")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> SyncGroupData()
        {
            try
            {
                await _contactService.SyncGroupParticipantData();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error($"Error in GetContactById: {ex}");
                Console.WriteLine($"Error in GetContactById: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
