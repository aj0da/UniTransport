using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTransport.BLL.ModelVM.TripVM;
using UniTransport.BLL.Service.Interface;
using UniTransport.DAL.Entities;
using UniTransport.DAL.Repository.Interface;

namespace UniTransport.BLL.Service.Implementation
{
    public class BookingService : IBookingService
    {
        private readonly ITripRepository _tripRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public BookingService(
            ITripRepository tripRepository,
            IBookingRepository bookingRepository,
            IMapper mapper)
        {
            _tripRepository = tripRepository;
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public async Task<List<TripViewModel>> GetAvailableTripsAsync(DateTime date)
        {
            var trips = await _tripRepository.GetAvailableTripsForDateAsync(date);
            return _mapper.Map<List<TripViewModel>>(trips);
        }

        public async Task<bool> CreateBookingAsync(string userId, List<string> tripIds)
        {
            if (!await ValidateBookingAsync(userId, tripIds))
                return false;

            var bookings = tripIds.Select(tripId => new Booking
            {
                Student = new Student { UserId = userId },
                TripId = int.Parse(tripId),
                BookingTime = DateTime.Now,
                IsCancelled = false
            }).ToList();

            return await _bookingRepository.CreateBookingsAsync(bookings);
        }
        
        public async Task<decimal> CalculateTotalPriceAsync(List<string> tripIds)
        {
            var trips = await _tripRepository.GetTripsByIdsAsync(tripIds);
            return (decimal)trips.Sum(t => t.Price);
        }

        public async Task<bool> ValidateBookingAsync(string userId, List<string> tripIds)
        {
            var trips = await _tripRepository.GetTripsByIdsAsync(tripIds);

            // Validate all trips exist
            if (trips.Count != tripIds.Count)
                return false;

            // Validate seats availability
            foreach (var trip in trips)
            {
                if (trip.AvailableSeats <= 0)
                    return false;
            }

            // Validate user doesn't have conflicting bookings
            var userBookings = await _bookingRepository.GetUserBookingsForDateAsync(userId, trips.First().DepartureTime);
            var hasConflict = userBookings.Any(b => tripIds.Contains(b.TripId.ToString()));

            return !hasConflict;
        }
    }
}
