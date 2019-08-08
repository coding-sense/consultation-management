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
    public class UserProfileController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;

        public UserProfileController(UserManager<ApplicationUser> userManager, IUserService userService)
        {
            _userManager = userManager;
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        //GET : abc.com/api/UserProfile
        public async Task<Object> GetUserProfile()
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            return new
            {
                FullName = user.FirstName + "" + user.LastNmae,
                user.Email,
                user.UserName
            };
        }

        [HttpGet]
        [Authorize]
        [Route("navigation-menu")]
        public async Task<Object> GetUsersNavigationMenus()
        {
            var userID = User.Claims.FirstOrDefault(p => p.Type == "UserID").Value;
            var result = await _userService.GetUserNavigationMenus(userID);
            return result;
        }
    }
}