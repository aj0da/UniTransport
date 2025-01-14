using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UniTransport.BLL.Service.Interface;
using UniTransport.DAL.Entities;
using Microsoft.AspNetCore.Http;

namespace UniTransport.BLL.Service.Implementation
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;

        public UserService(
            IHttpContextAccessor httpContextAccessor,
            UserManager<User> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task<User> GetCurrentUserAsync()
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
                return null;

            return await _userManager.FindByIdAsync(userId);
        }

        public bool IsAuthenticated()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        }

        public async Task<bool> IsInRoleAsync(string role)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
                return false;

            return await _userManager.IsInRoleAsync(user, role);
        }
    }
}
