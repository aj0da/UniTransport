using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTransport.BLL.ModelVM.TripVM;
using UniTransport.DAL.Entities;
using UniTransport.DAL.Enum;

namespace UniTransport.BLL.Service.Interface
{
    public interface ITripService
    {
        Task<IEnumerable<Trip>> GetAllTripsAsync();
        Task<Trip?> GetTripByIdAsync(int id);
        Task<bool> CreateTripAsync(CreateTripVM model);
        Task<bool> UpdateTripAsync(EditTripVM model);
        Task<bool> UpdateTripStatusAsync(int id, TripStatus status);
        Task<IEnumerable<Trip>> GetTripsByDateAsync(DateTime date);
        Task<IEnumerable<Trip>> GetTripsByVehicleIdAsync(int vehicleId);
        Task<IEnumerable<RequestedTrip>> GetPendingTripRequestsAsync();
        Task<bool> ApproveTripRequestAsync(int id);
        Task<bool> RejectTripRequestAsync(int id);
        IEnumerable<BookTripsVM> GetIncomingTrips();
        bool BookTrip(IEnumerable<BookTripsVM> trips, string userId);
	}
}
