using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultationManagmentAPI.Models
{
    public class IssueType
    {
        public int IssueTypeID { get; set; }
        public string Name { get; set; }
        public int Weightage { get; set; }
    }
}
