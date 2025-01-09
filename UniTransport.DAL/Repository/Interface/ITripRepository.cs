using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTransport.DAL.Entities;
using UniTransport.DAL.Enum;

namespace UniTransport.DAL.Repository.Interface
{
    public interface ITripRepository : IGenericRepository<Trip>
    {
        Task<Trip> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Trip>> GetActiveTripsAsync();
        Task<IEnumerable<Trip>> GetTripsByVehicleIdAsync(int vehicleId);
        Task<bool> UpdateTripStatusAsync(int tripId, TripStatus status);
        Task<bool> UpdateAvailableSeatsAsync(int tripId, int seats);
    }
}
