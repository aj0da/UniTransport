using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniTransport.BLL.Service.Interface;

namespace UniTransport.PLL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminBookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly ITripService _tripService;

        public AdminBookingController(IBookingService bookingService, ITripService tripService)
        {
            _bookingService = bookingService;
            _tripService = tripService;
        }

        // GET: AdminBooking
        public async Task<IActionResult> Index()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return View(bookings);
        }

        // GET: AdminBooking/TripBookings/5
        public async Task<IActionResult> TripBookings(int tripId)
        {
            var bookings = await _bookingService.GetBookingsByTripAsync(tripId);
            ViewBag.Trip = await _tripService.GetTripByIdAsync(tripId);
            return View(bookings);
        }

        // GET: AdminBooking/StudentBookings/5
        public async Task<IActionResult> StudentBookings(int studentId)
        {
            var bookings = await _bookingService.GetStudentBookingsAsync(studentId);
            return View(bookings);
        }

        // POST: AdminBooking/CancelBooking/5
        [HttpPost]
        public async Task<IActionResult> CancelBooking(int id)
        {
            await _bookingService.CancelBookingAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: AdminBooking/DailyBookings
        public async Task<IActionResult> DailyBookings(DateTime? date)
        {
            date ??= DateTime.Today;
            var bookings = await _bookingService.GetBookingsByDateAsync(date.Value);
            ViewBag.SelectedDate = date.Value;
            return View(bookings);
        }

        // GET: AdminBooking/BookingStatistics
        public async Task<IActionResult> BookingStatistics()
        {
            var stats = await _bookingService.GetBookingStatisticsAsync();
            return View(stats);
        }
    }
}
