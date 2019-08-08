using ConsultationManagmentAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultationManagmentAPI.Controllers.Services.User
{
    public class UserService : IUserService
    {
        private readonly DBContext dBContext;
        private UserManager<ApplicationUser> userManager;

        public UserService(DBContext _dBContext, UserManager<ApplicationUser> userManager)
        {
            dBContext = _dBContext;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsers(int? type = null)
        {
            var users = await userManager.Users.Where(p => p.Type == type).ToListAsync();
            return users;
        }

        public async Task<IEnumerable<NavigationMenu>> GetUserNavigationMenus(string userID)
        {
            var user = await userManager.FindByIdAsync(userID);
            var roles = (await userManager.GetRolesAsync(user)).ToArray();

            List<RoleNavgationMenu> menus = new List<RoleNavgationMenu>()
            {
                new RoleNavgationMenu(){
                    MenuName ="My Request",
                    Url ="/home/my-request",
                    RoleIDs =new List<string>(){  "Student","Teacher","Admin1"}
                },
                new RoleNavgationMenu(){
                    MenuName ="Create Appointment Request",
                    Url ="/home/create-request",
                    RoleIDs =new List<string>(){  "Student","Admin1"}
                },
                new RoleNavgationMenu(){
                    MenuName ="Urgent Request",
                    Url ="/home/urgent-request",
                    RoleIDs =new List<string>(){  "Admin1", "Teacher" }
                },
                new RoleNavgationMenu(){
                    MenuName ="Registration",
                    Url ="/home/registration",
                    RoleIDs =new List<string>(){  "Admin"}
                }
            };
            return menus.Where(p => p.RoleIDs.Any(q => roles.Contains(q)))
                .Select(p => new NavigationMenu()
                {
                    MenuName = p.MenuName,
                    Url = p.Url,

                });
        }

    }

    public class RoleNavgationMenu
    {
        public IEnumerable<string> RoleIDs { get; set; }
        public string MenuName { get; set; }
        public string Url { get; set; }
    }
}
