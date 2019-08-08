using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultationManagmentAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int Type { get; set; }
        [Column(TypeName = "nvarchar(150)")]
        public string FirstName { get; set; }
        [Column(TypeName = "nvarchar(150)")]
        public string LastNmae { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? CGPA { get; set; }
        public virtual ICollection<Request> FromRequests { get; set; } = new List<Request>();
        public virtual ICollection<Request> ToRequests { get; set; } = new List<Request>();
        public virtual ICollection<TimeSlot> TimeSlots { get; set; }

    }
}
