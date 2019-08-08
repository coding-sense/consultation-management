using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultationManagmentAPI.Models
{
    public class TimeSlot
    {
        [Key]
        public int TimeSlotID { get; set; }
        public string UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
    }
}
