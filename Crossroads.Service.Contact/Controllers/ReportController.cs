using Crossroads.Service.Contact.Interfaces;
using Crossroads.Service.Contact.Models;
using Crossroads.Web.Auth.Controllers;
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
    public class ReportController : AuthBaseController
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        readonly IReportService _reportService;

        public ReportController(IAuthTokenExpiryService authTokenExpiryService,
            IAuthenticationRepository authenticationRepository,
            IReportService reportService)
            : base(authenticationRepository, authTokenExpiryService)
        {
            _reportService = reportService;
        }

        /// <summary>
        ///  Get list of attendance summary report objects for an event
        /// </summary>
        [HttpGet]
        [Route("attendancesummary/{eventId}")]
        [ProducesResponseType(typeof(AttendanceSummaryDto), 200)]
        public async Task<IActionResult> GetAttendanceSummary(int eventId)
        {
            try
            {
                return Ok(await _reportService.GetAttendanceSummary(eventId));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error in GetAttendanceSummary: {ex}");
                Console.WriteLine($"Error in GetAttendanceSummary: {ex.Message}");
                return StatusCode(500, ex);
            }
        }
    }
}
