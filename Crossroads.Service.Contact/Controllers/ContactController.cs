using Crossroads.Web.Auth.Controllers;
using Crossroads.Web.Common.Auth.Helpers;
using Crossroads.Web.Common.Security;
using Crossroads.Web.Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System;

namespace Crossroads.Service.Contact.Controllers
{
    [Route("api/[controller]")]
    [RequiresAuthorization]
    public class ContactController : AuthBaseController
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public ContactController(IAuthTokenExpiryService authTokenExpiryService,
            IAuthenticationRepository authenticationRepository)
            : base(authenticationRepository, authTokenExpiryService)
        {
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
    }
}
