using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ConsultationManagmentAPI.Controllers.Services.User;
using ConsultationManagmentAPI.Models;
using ConsultationManagmentAPI.Models.Entity;
using ConsultationManagmentAPI.Models.Extra;
using ConsultationManagmentAPI.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ConsultationManagmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private ApplicationUserManger _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationSettings _appSettings;
        private IUserService _userService;
        

        public UserController(ApplicationUserManger userManager, SignInManager<ApplicationUser> signInManager,
            IOptions<ApplicationSettings> appSettings, IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
            _userService = userService;
        }
        private string GetRoleNameByType(int typeID)
        {
            string roleName = "";
            switch (typeID)
            {
                case 1:
                    roleName = "Admin";
                    break;
                case 2:
                    roleName = "Teacher";
                    break;
                case 3:
                    roleName = "Student";
                    break;
            };
            return roleName;
        }

        [HttpPost]
        [Authorize]
        [Route("register")]
        //http://abc.com/api/user/register
        public async Task<Object> RegisterUser(UserModel model)
        {
            var app = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastNmae = model.LastName,
                Type = model.Type,
                CGPA =model.CGPA
            };
            try
            {
                var result = await _userManager.CreateAsync(app, model.Password);
                var roleName = GetRoleNameByType(model.Type);
                result = await _userManager.AddToRoleAsync(app, roleName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        [Route("login")]
        //POST : /api/ApplicationUser/Login
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var role = await _userManager.GetRolesAsync(user);
                IdentityOptions _options = new IdentityOptions();

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID",user.Id.ToString()),
                        new Claim(_options.ClaimsIdentity.RoleClaimType,role.FirstOrDefault())
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtSecretKey)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return Ok(new { token });
            }
            else
                return BadRequest(new { message = "Username or password is incorrect." });
        }


        [HttpGet]
        [Authorize(Roles = "Student,Admin")]
        [Route("teachers")]
        public async Task<IEnumerable<ApplicationUser>> GetTeachers()
        {
            var userID = User.Claims.FirstOrDefault(p => p.Type == "UserID").Value;
            var result = await _userService.GetUsers(2);
            return result;
        }
    }
}