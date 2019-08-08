using ConsultationManagmentAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultationManagmentAPI.Controllers.Services.User
{
    public interface IUserService
    {
        Task<IEnumerable<NavigationMenu>> GetUserNavigationMenus(string userID);
        Task<IEnumerable<ApplicationUser>> GetUsers(int? type = null);
    }
}
