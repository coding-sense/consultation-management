using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultationManagmentAPI.Models.Commands.Requests
{
    public class CreateRequestCommand
    {
        public int IssueTypeID { get; set; }
        public string Issue { get; set; }
        public string FromUserID { get; set; }
        public string ToUserID { get; set; }
        public int TimeSlotID { get; set; }
    }
}
