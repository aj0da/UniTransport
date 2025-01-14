using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniTransport.BLL.ModelVM.TripVM;
using UniTransport.BLL.Service.Interface;
using UniTransport.DAL.Enum;

namespace UniTransport.PLL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminScheduleController : Controller
    {
        private readonly ITripService _tripService;
        private readonly IVehicleService _vehicleService;

        public AdminScheduleController(ITripService tripService, IVehicleService vehicleService)
        {
            _tripService = tripService;
            _vehicleService = vehicleService;
        }

        // GET: AdminSchedule
        public async Task<IActionResult> Index()
        {
            var trips = await _tripService.GetAllTripsAsync();
            return View(trips);
        }

        // GET: AdminSchedule/Create
        public async Task<IActionResult> Create()
        {
            // Get available vehicles for dropdown
            ViewBag.Vehicles = await _vehicleService.GetActiveVehiclesAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTripVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Vehicles = await _vehicleService.GetActiveVehiclesAsync();
                return View(model);
            }

            try
            {
                await _tripService.CreateTripAsync(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Failed to create trip: " + ex.Message);
                ViewBag.Vehicles = await _vehicleService.GetActiveVehiclesAsync();
                return View(model);
            }
        }

        // GET: AdminSchedule/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var trip = await _tripService.GetTripByIdAsync(id);
            if (trip == null)
                return NotFound();

            ViewBag.Vehicles = await _vehicleService.GetActiveVehiclesAsync();
            return View(trip);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditTripVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Vehicles = await _vehicleService.GetActiveVehiclesAsync();
                return View(model);
            }

            try
            {
                await _tripService.UpdateTripAsync(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Failed to update trip: " + ex.Message);
                ViewBag.Vehicles = await _vehicleService.GetActiveVehiclesAsync();
                return View(model);
            }
        }

        // POST: AdminSchedule/UpdateStatus/5
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, TripStatus status)
        {
            await _tripService.UpdateTripStatusAsync(id, status);
            return RedirectToAction(nameof(Index));
        }

        // GET: AdminSchedule/DailySchedule
        public async Task<IActionResult> DailySchedule(DateTime? date)
        {
            date ??= DateTime.Today;
            var trips = await _tripService.GetTripsByDateAsync(date.Value);
            ViewBag.SelectedDate = date.Value;
            return View(trips);
        }

        // GET: AdminSchedule/RequestedTrips
        public async Task<IActionResult> RequestedTrips()
        {
            var requests = await _tripService.GetPendingTripRequestsAsync();
            return View(requests);
        }

        // POST: AdminSchedule/ApproveRequest/5
        [HttpPost]
        public async Task<IActionResult> ApproveRequest(int id)
        {
            await _tripService.ApproveTripRequestAsync(id);
            return RedirectToAction(nameof(RequestedTrips));
        }

        // POST: AdminSchedule/RejectRequest/5
        [HttpPost]
        public async Task<IActionResult> RejectRequest(int id)
        {
            await _tripService.RejectTripRequestAsync(id);
            return RedirectToAction(nameof(RequestedTrips));
        }
    }
}
