using ConsultationManagmentAPI.Models;
using ConsultationManagmentAPI.Models.Commands.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultationManagmentAPI.Controllers.Services.Requests
{
    public interface IRequestService
    {
        Task<IEnumerable<UserRequest>> GetUsersRequests(string userID);

        Task<int> CreateRequest(CreateRequestCommand command);

        Task<int> ChangeStatus(int requestID, int status);

        bool CanupdateRequest(int requestID, string userID);
        Task<IEnumerable<IssueType>> GetIssueTypes();

        Task<IEnumerable<UrgentUserRequest>> GetUrgentUsersRequests(string userID);
    }
}
