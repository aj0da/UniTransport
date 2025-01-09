using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTransport.DAL.Database;
using UniTransport.DAL.Entities;
using UniTransport.DAL.Repository.Interface;

namespace UniTransport.DAL.Repository.Implementation
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }

        public async Task<User> GetByEmailAsync(string email)
        {
            try
            {
                return await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> DeactivateUserAsync(string userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null) return false;
                user.IsDeleted = true;
                return await UpdateAsync(user);
            }
            catch
            {
                return false;
            }
        }
    }
}
