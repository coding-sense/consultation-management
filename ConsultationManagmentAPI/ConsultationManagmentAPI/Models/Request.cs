using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultationManagmentAPI.Models
{
    public class Request
    {
        [Key]
        public int RequestID { get; set; }
        public int IssueTypeID { get; set; }
        public string Issue { get; set; }
        public virtual IssueType IssueType { get; set; }
        public string FromUserID { get; set; }
        [ForeignKey("FromUserID")]
        public virtual ApplicationUser FromUser { get; set; }
        [ForeignKey("ToUserID")]
        public string ToUserID { get; set; }
        public virtual ApplicationUser ToUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ApproveDate { get; set; }
        public int TimeSlotID { get; set; }
        public virtual TimeSlot TimeSlot { get; set; }
        public int Status { get; set; }
    }
}
