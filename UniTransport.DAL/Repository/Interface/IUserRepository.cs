using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTransport.DAL.Entities;

namespace UniTransport.DAL.Repository.Interface
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task<bool> DeactivateUserAsync(string userId);
    }
}
