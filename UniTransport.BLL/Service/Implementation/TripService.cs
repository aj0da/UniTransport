using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTransport.BLL.ModelVM.TripVM;
using UniTransport.BLL.Service.Interface;
using UniTransport.DAL.Entities;
using UniTransport.DAL.Enum;
using UniTransport.DAL.Repository.Interface;

namespace UniTransport.BLL.Service.Implementation
{
	public class TripService : ITripService
	{
		private readonly ITripRepository _tripRepository;
        private readonly IRequestedTripRepository _requestedTripRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IBookingRepository _bookingRepository;
		private readonly IStudentRepository _studentRepository;
		private readonly IMapper _mapper;
        public TripService(ITripRepository repository, IBookingRepository bookingRepository, IMapper mapper, IStudentRepository studentRepository, IVehicleRepository vehicleRepository, IRequestedTripRepository requestedTripRepository)
        {
            _tripRepository = repository;
            _bookingRepository = bookingRepository;
            _mapper = mapper;
            _studentRepository = studentRepository;
            _vehicleRepository = vehicleRepository;
            _requestedTripRepository = requestedTripRepository;
        }

        public async Task<IEnumerable<Trip>> GetAllTripsAsync()
        {
            return await _tripRepository.GetAllAsync();
        }

        public async Task<Trip?> GetTripByIdAsync(int id)
        {
            return await _tripRepository.GetByIdAsync(id);
        }

        public async Task<bool> CreateTripAsync(CreateTripVM model)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(model.VehicleId);
            if (vehicle == null || !vehicle.IsActive)
                return false;

            var trip = new Trip
            {
                DepartureLocation = model.DepartureLocation,
                ArrivalLocation = model.ArrivalLocation,
                DepartureTime = model.DepartureTime,
                ArrivalTime = model.ArrivalTime,
                AvailableSeats = model.AvailableSeats,
                Price = model.Price,
                VehicleId = model.VehicleId,
                TripStatus = model.TripStatus,
                RequestedTripId = model.RequestedTripId
            };

            return await _tripRepository.CreateAsync(trip);
        }

        public async Task<bool> UpdateTripAsync(EditTripVM model)
        {
            var trip = await _tripRepository.GetByIdAsync(model.TripId);
            if (trip == null)
                return false;

            var vehicle = await _vehicleRepository.GetByIdAsync(model.VehicleId);
            if (vehicle == null || !vehicle.IsActive)
                return false;

            trip.DepartureLocation = model.DepartureLocation;
            trip.ArrivalLocation = model.ArrivalLocation;
            trip.DepartureTime = model.DepartureTime;
            trip.ArrivalTime = model.ArrivalTime;
            trip.AvailableSeats = model.AvailableSeats;
            trip.Price = model.Price;
            trip.VehicleId = model.VehicleId;
            trip.TripStatus = model.TripStatus;

            return await _tripRepository.UpdateAsync(trip);
        }

        public async Task<bool> UpdateTripStatusAsync(int id, TripStatus status)
        {
            var trip = await _tripRepository.GetByIdAsync(id);
            if (trip == null)
                return false;

            trip.TripStatus = status;
            return await _tripRepository.UpdateAsync(trip);
        }

        public async Task<IEnumerable<Trip>> GetTripsByDateAsync(DateTime date)
        {
            var trips = await _tripRepository.GetAllAsync();
            return trips.Where(t => t.DepartureTime.Date == date.Date);
        }

        public async Task<IEnumerable<RequestedTrip>> GetPendingTripRequestsAsync()
        {
            var requests = await _requestedTripRepository.GetAllAsync();
            return requests.Where(r => r.Status == RequestedTripStatus.Pending);
        }

        public async Task<bool> ApproveTripRequestAsync(int id)
        {
            var request = await _requestedTripRepository.GetByIdAsync(id);
            if (request == null)
                return false;

            request.Status = RequestedTripStatus.Approved;
            return await _requestedTripRepository.UpdateAsync(request);
        }

        public async Task<bool> RejectTripRequestAsync(int id)
        {
            var request = await _requestedTripRepository.GetByIdAsync(id);
            if (request == null)
                return false;

            request.Status = RequestedTripStatus.Rejected;
            return await _requestedTripRepository.UpdateAsync(request);
        }

        //Get active incoming trips which its time after 10m from know
        public IEnumerable<BookTripsVM> GetIncomingTrips()
		{
			var incomingTrips = _tripRepository.GetIncomingTripsAsync().Result;
			return _mapper.Map<IEnumerable<BookTripsVM>>(incomingTrips);
		}
		public bool BookTrip(IEnumerable<BookTripsVM> trips, string userId)
		{
			foreach (var trip in trips)
			{
				if (trip.Isbooked)
				{
					try
					{
						var studentId = _studentRepository.GetByUserIdAsync(userId).Result.StudentId;
						var book = new Booking { TripId = trip.TripId, StudentId = studentId };
						var x = _bookingRepository.CreateAsync(book).Result;

						trip.AvailableSeats--;
						if (trip.AvailableSeats == 0)
						{
							var y = _tripRepository.UpdateTripStatusAsync(trip.TripId, TripStatus.Completed).Result;
						}
						var z = _tripRepository.UpdateAvailableSeatsAsync(trip.TripId, trip.AvailableSeats).Result;
						return true;
					}
					catch(Exception ex)
					{ 
						return false;
					}	
				}
			}
			return true;
		}

        public async Task<IEnumerable<Trip>> GetTripsByVehicleIdAsync(int vehicleId)
        {
            var trips = await _tripRepository.GetAllAsync();
            return trips.Where(t => t.VehicleId == vehicleId);
        }

    }
}
