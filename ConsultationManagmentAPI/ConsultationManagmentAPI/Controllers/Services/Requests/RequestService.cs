using ConsultationManagmentAPI.Models;
using ConsultationManagmentAPI.Models.Commands.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ConsultationManagmentAPI.Controllers.Services.Requests
{
    public enum RequestStatus
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3,
        Served = 4
    }

    public class RequestService : IRequestService
    {
        private readonly DBContext dBContext;
        private UserManager<ApplicationUser> userManager;

        public RequestService(DBContext _dBContext, UserManager<ApplicationUser> userManager)
        {
            dBContext = _dBContext;
            this.userManager = userManager;
        }

        public bool CanupdateRequest(int requestID, string userID)
        {
            return dBContext.Requests.Any(p => p.RequestID == requestID && p.ToUserID == userID);
        }

        public async Task<IEnumerable<IssueType>> GetIssueTypes()
        {
            return await dBContext.IssueTypes.ToListAsync();
        }

        public async Task<IEnumerable<UrgentUserRequest>> GetUrgentUsersRequests(string userID)
        {
            var user = await userManager.FindByIdAsync(userID);
            var roles = (await userManager.GetRolesAsync(user)).ToArray();


            var request = from req in dBContext.Requests.Include(p => p.FromUser).ThenInclude(p => p.FromRequests)
                          .Include(p => p.ToUser).Include(p => p.IssueType)
                          .Where(p => p.Status == (int)RequestStatus.Pending && p.FromUser.CGPA != null)
                          let fromUser = req.FromUser
                          let previousCount = fromUser.FromRequests
                          .Where(p => p.Status == (int)RequestStatus.Approved || p.Status == (int)RequestStatus.Served).Count()
                          let cgpaWieght = fromUser.CGPA > 3.5M ? 5 : fromUser.CGPA > 3.0M && fromUser.CGPA <= 3.5M ? 4 : 3
                          select new UrgentUserRequest
                          {
                              CGPAWeight = cgpaWieght,
                              IssueTypeID = req.IssueTypeID,
                              Issue = req.Issue,
                              ServedCount = previousCount,
                              CreateDate = req.CreateDate,
                              EndDate = req.TimeSlot.EndDate,
                              StartDate = req.TimeSlot.StartDate,
                              FromName = req.FromUser.FirstName + " " + req.FromUser.LastNmae,
                              IssueType = req.IssueType.Name,
                              RequestID = req.RequestID,
                              endDateString = req.TimeSlot.EndDate.ToString("dd/MMM/yyyy"),
                              startDateString = req.TimeSlot.StartDate.ToString("dd/MMM/yyyy"),
                              startTimeString = req.TimeSlot.StartDate.ToString("hh:mm tt"),
                              endTimeString = req.TimeSlot.EndDate.ToString("hh:mm tt"),
                              CanApprove = roles.Any(q => q == "Teacher"),
                              Status = ((RequestStatus)req.Status).ToString()

                          };

            return await request.OrderBy(p => p.IssueTypeID)
                .ThenBy(p => p.ServedCount).ThenByDescending(p => p.CGPAWeight).ToListAsync();

        }

        public async Task<IEnumerable<UserRequest>> GetUsersRequests(string userID)
        {
            var user = await userManager.FindByIdAsync(userID);
            var roles = (await userManager.GetRolesAsync(user)).ToArray();

            var users = await dBContext.Requests.Include(p => p.TimeSlot).Include(p => p.FromUser)
                .Include(p => p.ToUser).Include(p => p.IssueType).Where(p => p.FromUserID == userID || p.ToUserID == userID)
                .ToListAsync();


            return users.Select(p => new UserRequest
            {
                ApproveDate = p.ApproveDate,
                CreateDate = p.CreateDate,
                EndDate = p.TimeSlot.EndDate,
                StartDate = p.TimeSlot.StartDate,
                FromName = p.FromUser.FirstName + " " + p.FromUser.LastNmae,
                ToName = p.ToUser.FirstName + " " + p.ToUser.LastNmae,
                FromUserID = p.FromUserID,
                ToUserID = p.ToUserID,
                Issue = p.Issue,
                IssueTypeID = p.IssueTypeID,
                IssueType = p.IssueType.Name,
                RequestID = p.RequestID,
                endDateString = p.TimeSlot.EndDate.ToString("dd/MMM/yyyy"),
                startDateString = p.TimeSlot.StartDate.ToString("dd/MMM/yyyy"),
                startTimeString = p.TimeSlot.StartDate.ToString("hh:mm tt"),
                endTimeString = p.TimeSlot.EndDate.ToString("hh:mm tt"),
                CanApprove = roles.Any(q => q == "Teacher"),
                Status = ((RequestStatus)p.Status).ToString()

            });
        }

        public async Task<int> CreateRequest(CreateRequestCommand command)
        {
            Request request = new Request();
            request.CreateDate = System.DateTime.Now;
            request.FromUserID = command.FromUserID;
            request.Issue = command.Issue;
            request.IssueTypeID = command.IssueTypeID;
            request.TimeSlotID = command.TimeSlotID;
            request.ToUserID = command.ToUserID;
            request.Status = (int)RequestStatus.Pending;
            dBContext.Requests.Add(request);

            await dBContext.SaveChangesAsync();
            return request.RequestID;
        }

        public async Task<int> ChangeStatus(int requestID, int status)
        {
            var request = await dBContext.Requests.Include(p => p.FromUser)
                .Include(p => p.ToUser).Where(p => p.RequestID == requestID).FirstOrDefaultAsync();
            request.Status = status;
            if (status == (int)RequestStatus.Approved)
            {
                request.ApproveDate = System.DateTime.Now;
            }
            else
            {
                request.ApproveDate = null;
            }
            await dBContext.SaveChangesAsync();

            if (request.FromUser != null && request.FromUser.Email != null && request.ToUser != null)
            {
                SendEmail(request.FromUser.Email, "Status Chnage Email",
                    "Your request status (for " + request.ToUser.FirstName + " " + request.ToUser.LastNmae+ ")  has changed to " + ((RequestStatus)status).ToString());
            }

            return request.RequestID;
        }

        public void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                // Credentials
                var credentials = new NetworkCredential("project.ned11@gmail.com", "abc@123465");

                // Mail message
                var mail = new MailMessage()
                {
                    From = new MailAddress("project.ned11@gmail.com"),
                    Subject = subject,
                    Body = body
                };

                mail.To.Add(new MailAddress(toEmail));

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

                // Send it...         
                client.Send(mail);
            }

            catch (Exception ex)
            {

            }
        }
    }




    public class UserRequest
    {
        public int RequestID { get; set; }
        public string IssueType { get; set; }
        public int IssueTypeID { get; set; }
        public string Issue { get; set; }
        public string FromUserID { get; set; }
        public string FromName { get; set; }
        public string ToUserID { get; set; }
        public string ToName { get; set; }
        public string startDateString { get; set; }
        public string endDateString { get; set; }
        public string startTimeString { get; set; }
        public string endTimeString { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ApproveDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool CanApprove { get; set; }
        public string Status { get; set; }
        public decimal CGPA { get; set; }

    }

    public class UrgentUserRequest
    {
        public int RequestID { get; set; }
        public int IssueTypeID { get; set; }
        public string IssueType { get; set; }
        public string Issue { get; set; }
        public string FromName { get; set; }
        public string ToName { get; set; }
        public string startDateString { get; set; }
        public string endDateString { get; set; }
        public string startTimeString { get; set; }
        public string endTimeString { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ApproveDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool CanApprove { get; set; }
        public string Status { get; set; }
        public decimal CGPA { get; set; }
        public decimal CGPAWeight { get; set; }
        public int ServedCount { get; set; }
    }
}
