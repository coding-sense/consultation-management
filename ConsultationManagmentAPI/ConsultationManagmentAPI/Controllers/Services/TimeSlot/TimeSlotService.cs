using ConsultationManagmentAPI.Controllers.Services.Requests;
using ConsultationManagmentAPI.Models;
using ConsultationManagmentAPI.Models.Commands.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultationManagmentAPI.Controllers.Services.TimeSlots
{
    public class TimeSlotService : ITimeSlotService
    {
        private readonly DBContext dBContext;
        private UserManager<ApplicationUser> userManager;

        public TimeSlotService(DBContext _dBContext, UserManager<ApplicationUser> userManager)
        {
            dBContext = _dBContext;
            this.userManager = userManager;
        }

        private static bool IsIntervalsOverlap(DateTime? startA, DateTime? endA, DateTime? startB, DateTime? endB)
        {
            return (startA == null || endB == null || startA <= endB) && (endA == null || startB == null || endA >= startB);
        }

        private static bool IsTimeOverlap(TimeSpan? startA, TimeSpan? endA, TimeSpan? startB, TimeSpan? endB)
        {
            return (startA == null || endB == null || startA <= endB) && (endA == null || startB == null || endA >= startB);
        }

        public async Task<IEnumerable<string>> GetTeachersAvailibility(int timeSlotID)
        {
            List<string> issue = new List<string>();
            var timeSlot = dBContext.TimeSlots.FirstOrDefault(p => p.TimeSlotID == timeSlotID);
            var invalidTimeSlot = await dBContext.Requests.Where(p => p.ToUserID == timeSlot.UserId && (p.Status == (int)RequestStatus.Pending ||
            p.Status == (int)RequestStatus.Rejected)).Include(p => p.TimeSlot).Select(p => p.TimeSlot).ToListAsync();

            var uresponsiveTimSlots = invalidTimeSlot.Where(p => IsTimeOverlap(timeSlot.StartDate.TimeOfDay, timeSlot.EndDate.TimeOfDay, p.StartDate.TimeOfDay, p.EndDate.TimeOfDay));
            var isoverLapping = invalidTimeSlot.Any(p => IsIntervalsOverlap(timeSlot.StartDate, timeSlot.EndDate, p.StartDate, p.EndDate));
            if (uresponsiveTimSlots.Count() > 5)
            {
                issue.Add("Warning! This teach is usually not reponsive during this time");
            }
            if (isoverLapping)
            {
                issue.Add("Error! Time slot is overlapping");
            }
            return issue;
        }

        public async Task<IEnumerable<TimeSlotRM>> GetUsersTimeSlot(string userID)
        {
            var users = await userManager.Users.Include(p => p.TimeSlots)
                .Where(p => p.Id == userID).SelectMany(p => p.TimeSlots).Include(p => p.Requests)
                .Where(p => (p.Requests.Count() == 0 || p.Requests.Any(q => q.Status != 1)) && System.DateTime.Now >= p.StartDate && System.DateTime.Now <= p.EndDate)
                .ToListAsync();
            return users.Select(p => new TimeSlotRM()
            {
                EndDate = p.EndDate,
                StartDate = p.StartDate,
                TimeSlotID = p.TimeSlotID,
                UserID = p.UserId,
                DateString = p.StartDate.ToString("dd/MMM/yyyy"),
                TimeFrom = p.StartDate.ToString("hh:mm tt"),
                TimeTo = p.EndDate.ToString("hh:mm tt"),

            }).OrderBy(p => p.StartDate).ThenBy(p => p.EndDate);
        }

    }

    public class TimeSlotRM
    {
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
        public string DateString { get; set; }
        public int TimeSlotID { get; set; }
        public DateTime StartDate { get; set; }
        public string UserID { get; set; }
        public DateTime EndDate { get; set; }

    }
}
