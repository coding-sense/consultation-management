using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultationManagmentAPI.Models.Requests
{
    public class CreateUserRequest
    {
        public int TeacherID { get; set; }
        public int TimeSlotID { get; set; }
        public int IssueTypeID { get; set; }
        public string Issue { get; set; }

    }
}
