using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTransport.DAL.Entities;

namespace UniTransport.DAL.Repository.Interface
{
    public interface IStudentRepository : IGenericRepository<Student>
    {
        Task<Student> GetByIdWithDetailsAsync(int id);
        Task<Student> GetByUniversityIdAsync(int universityStudentId);
        Task<Student> GetByUserIdAsync(string userId);
    }
}
