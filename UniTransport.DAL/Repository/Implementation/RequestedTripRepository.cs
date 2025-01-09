using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTransport.DAL.Database;
using UniTransport.DAL.Entities;
using UniTransport.DAL.Enum;
using UniTransport.DAL.Repository.Interface;

namespace UniTransport.DAL.Repository.Implementation
{
    public class RequestedTripRepository : GenericRepository<RequestedTrip>, IRequestedTripRepository
    {
        public RequestedTripRepository(ApplicationDbContext context) : base(context) { }

        public async Task<RequestedTrip> GetByIdWithDetailsAsync(int id)
        {
            return await _context.RequestedTrips
                .Include(rt => rt.Student)
                    .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(rt => rt.RequestedTripId == id);
        }

        public async Task<IEnumerable<RequestedTrip>> GetRequestedTripsByStudentIdAsync(int studentId)
        {
            return await _context.RequestedTrips
                .Where(rt => rt.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<RequestedTrip>> GetPendingRequestsAsync()
        {
            return await _context.RequestedTrips
                .Include(rt => rt.Student)
                    .ThenInclude(s => s.User)
                .Where(rt => rt.Status == RequestedTripStatus.Pending)
                .ToListAsync();
        }

        public async Task<bool> UpdateStatusAsync(int requestedTripId, RequestedTripStatus status)
        {
            var requestedTrip = await GetByIdAsync(requestedTripId);
            if (requestedTrip == null) return false;

            try
            {
                requestedTrip.Status = status;
                return await UpdateAsync(requestedTrip);
            }
            catch
            {
                return false;
            }
        }
    }
}
