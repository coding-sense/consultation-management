using ConsultationManagmentAPI.Controllers.Services.Requests;
using ConsultationManagmentAPI.Models;
using ConsultationManagmentAPI.Models.Commands.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultationManagmentAPI.Controllers.Services.TimeSlots
{
    public interface ITimeSlotService
    {
        Task<IEnumerable<TimeSlotRM>> GetUsersTimeSlot(string userID);
        Task<IEnumerable<string>> GetTeachersAvailibility (int timeSlotID);
    }
}
