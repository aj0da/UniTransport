using AutoMapper;
using UniTransport.BLL.ModelVM.BookingVM;
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

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _bookingRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByTripAsync(int tripId)
        {
            var bookings = await _bookingRepository.GetAllAsync();
            return bookings.Where(b => b.TripId == tripId);
        }

        public async Task<IEnumerable<Booking>> GetStudentBookingsAsync(int studentId)
        {
            var bookings = await _bookingRepository.GetAllAsync();
            return bookings.Where(b => b.StudentId == studentId);
        }

        public async Task<bool> CancelBookingAsync(int id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
                return false;

            booking.IsCancelled = true;
            return await _bookingRepository.UpdateAsync(booking);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByDateAsync(DateTime date)
        {
            var bookings = await _bookingRepository.GetAllAsync();
            var trips = await _tripRepository.GetAllAsync();

            return bookings.Where(b => 
                trips.Any(t => t.TripId == b.TripId && t.DepartureTime.Date == date.Date));
        }

        public async Task<BookingStatisticsVM> GetBookingStatisticsAsync()
        {
            var bookings = await _bookingRepository.GetAllAsync();
            var trips = await _tripRepository.GetAllAsync();

            var stats = new BookingStatisticsVM
            {
                TotalBookings = bookings.Count(),
                ActiveBookings = bookings.Count(b => !b.IsCancelled),
                CancelledBookings = bookings.Count(b => b.IsCancelled),
                TotalRevenue = bookings
                    .Where(b => !b.IsCancelled)
                    .Join(trips, b => b.TripId, t => t.TripId, (b, t) => (decimal)t.Price)
                    .Sum(),
                TotalPassengers = bookings.Count(b => !b.IsCancelled),
            };

            // Group bookings by route
            stats.BookingsByRoute = bookings
                .Where(b => !b.IsCancelled)
                .Join(trips, b => b.TripId, t => t.TripId, (b, t) => $"{t.DepartureLocation} - {t.ArrivalLocation}")
                .GroupBy(r => r)
                .ToDictionary(g => g.Key, g => g.Count());

            // Group bookings by vehicle
            stats.BookingsByVehicle = bookings
                .Where(b => !b.IsCancelled)
                .Join(trips, b => b.TripId, t => t.TripId, (b, t) => t.Vehicle?.LicensePlate ?? "Unknown")
                .GroupBy(v => v)
                .ToDictionary(g => g.Key, g => g.Count());

            // Group bookings by date
            stats.BookingsByDate = bookings
                .Where(b => !b.IsCancelled)
                .Join(trips, b => b.TripId, t => t.TripId, (b, t) => t.DepartureTime.Date)
                .GroupBy(d => d)
                .ToDictionary(g => g.Key, g => g.Count());

            return stats;
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
