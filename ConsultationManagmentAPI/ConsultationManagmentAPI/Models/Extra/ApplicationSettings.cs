using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultationManagmentAPI.Models.Extra
{
    public class ApplicationSettings
    {
        public string JwtSecretKey { get; set; }
        public string ClientAppUrl { get; set; }
    }
}
