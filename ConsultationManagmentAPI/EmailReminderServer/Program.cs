using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmailReminderServer
{
    class Program
    {
        static void Main(string[] args)
        {
            SendReminderEmail();
        }

        private static void SendReminderEmail()
        {
            CMSEntities ent = new CMSEntities();
            var requests = ent.Requests.Where(p => p.Status == 2 && p.AspNetUser1.Email != null).ToList().Where(p => p.TimeSlot.StartDate.Date.AddDays(-1) == System.DateTime.Now.Date);
            // Credentials
            var credentials = new NetworkCredential("project.ned11@gmail.com", "abc@123465");

            // Smtp client
            var client = new SmtpClient()
            {
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = "smtp.gmail.com",
                EnableSsl = true,
                Credentials = credentials
            };



            foreach (var request in requests)
            {
                // Mail message
                var mail = new MailMessage()
                {
                    From = new MailAddress("project.ned11@gmail.com"),
                    Subject = "Reminder! ",
                    Body = "You have and upcomming consultation on " + request.TimeSlot.StartDate.ToString("dd/MMM/yyyy")
                    + " between " + request.TimeSlot.StartDate.ToString("hh:mm tt")
                    + "-" + request.TimeSlot.EndDate.ToString("hh:mm tt"),
                };
                mail.To.Add(new MailAddress(request.AspNetUser1.Email));
                client.Send(mail);
            }

            // Force a garbage collection to occur for this demo.
            GC.Collect();
        }
    }
}
