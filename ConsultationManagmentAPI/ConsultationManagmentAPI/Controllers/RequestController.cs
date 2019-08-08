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

namespace ConsultationManagmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        private readonly IRequestService _requestService;

        public RequestController(UserManager<ApplicationUser> userManager, IUserService userService, IRequestService requestService)
        {
            _userManager = userManager;
            _userService = userService;
            _requestService = requestService;
        }

        [HttpGet]
        [Authorize]
        [Route("issue-types")]
        public async Task<IEnumerable<IssueType>> GetIssueTypes()
        {
            return await _requestService.GetIssueTypes();
        }

        [HttpGet]
        [Authorize]
        [Route("my-requests")]
        public async Task<IEnumerable<UserRequest>> GetMyRequest()
        {
            var userID = User.Claims.FirstOrDefault(p => p.Type == "UserID").Value;
            return await _requestService.GetUsersRequests(userID);
        }

        [HttpGet]
        [Authorize(Roles = "Teacher")]
        [Route("urgent-requests")]
        public async Task<IEnumerable<UrgentUserRequest>> GetUsersRequests()
        {
            var userID = User.Claims.FirstOrDefault(p => p.Type == "UserID").Value;
            return await _requestService.GetUrgentUsersRequests(userID);
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        [Route("create-request")]
        public async Task<Object> CreateUserRequest([WebHTTP.FromUri] CreateRequestCommand request)
        {
            var userID = User.Claims.FirstOrDefault(p => p.Type == "UserID").Value;
            request.FromUserID = userID;
            var result = await _requestService.CreateRequest(request);
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "Teacher")]
        [Route("change-status/{requestID}")]
        public async Task<Object> ApproveUserRequest(int requestID, [WebHTTP.FromUri] StatusChangeRequest request)
        {
            var userID = User.Claims.FirstOrDefault(p => p.Type == "UserID").Value;
            if (_requestService.CanupdateRequest(requestID, userID))
            {
                var result = await _requestService.ChangeStatus(requestID, request.Status);
                return result;
            }
            else
            {
                return BadRequest("User is not allowed to approve time request");
            }
        }

    }

    public class StatusChangeRequest
    {
        public int Status { get; set; }
    }
}