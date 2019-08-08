using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebHTTP = System.Web.Http;
using ConsultationManagmentAPI.Controllers.Services;
using ConsultationManagmentAPI.Controllers.Services.Requests;
using ConsultationManagmentAPI.Controllers.Services.User;
using ConsultationManagmentAPI.Models;
using ConsultationManagmentAPI.Models.Commands.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ConsultationManagmentAPI.Controllers.Services.TimeSlots;

namespace ConsultationManagmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeSlotController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly ITimeSlotService _timeSlotService;

        public TimeSlotController(UserManager<ApplicationUser> userManager, ITimeSlotService timeSlotService)
        {
            _userManager = userManager;
            _timeSlotService = timeSlotService;
        }

        [HttpGet]
        [Authorize]
        [Route("{userId}")]
        public async Task<IEnumerable<TimeSlotRM>> GetUsersTimeSlot(string userID)
        {
            var result = await _timeSlotService.GetUsersTimeSlot(userID);
            return result;
        }

        [HttpGet]
        [Authorize]
        [Route("availibility/{timeSlotID}")]
        public async Task<IEnumerable<string>> GetTeachersAvailibility(int timeSlotID)
        {
            var result = await _timeSlotService.GetTeachersAvailibility( timeSlotID);
            return result;
        }
    }
}