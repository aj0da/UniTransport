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
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Student> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Students
                .Include(s => s.User)
                .Include(s => s.Bookings)
                .Include(s => s.RequestedTrips)
                .FirstOrDefaultAsync(s => s.StudentId == id);
        }

        public async Task<Student> GetByUniversityIdAsync(int universityStudentId)
        {
            return await _context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.UniversityStudentId == universityStudentId);
        }

        public async Task<Student> GetByUserIdAsync(string userId)
        {
            return await _context.Students
            .Include(s => s.User) // Include the User navigation property
            .Include(s => s.Bookings)
                .ThenInclude(b => b.Trip)
                    .ThenInclude(t => t.Vehicle) // Include nested properties for Bookings
            .Include(s => s.RequestedTrips) // Include RequestedTrips
            .Where(s => s.User.Id == userId) // Filter by UserId
            .FirstOrDefaultAsync();
        }
    }

}
