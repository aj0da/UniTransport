using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UniTransport.DAL.Entities;

namespace UniTransport.BLL.Service.Interface
{
    public interface IUserService
    {
        string GetCurrentUserId();
        Task<User> GetCurrentUserAsync();
        bool IsAuthenticated();
        Task<bool> IsInRoleAsync(string role);
    }
}
