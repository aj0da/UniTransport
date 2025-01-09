using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTransport.DAL.Entities;
using UniTransport.DAL.Enum;

namespace UniTransport.DAL.Repository.Interface
{
    public interface IRequestedTripRepository : IGenericRepository<RequestedTrip>
    {
        Task<RequestedTrip> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<RequestedTrip>> GetRequestedTripsByStudentIdAsync(int studentId);
        Task<IEnumerable<RequestedTrip>> GetPendingRequestsAsync();
        Task<bool> UpdateStatusAsync(int requestedTripId, RequestedTripStatus status);
    }
}
