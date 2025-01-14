using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniTransport.BLL.Service.Interface;
using UniTransport.DAL.Entities;
using UniTransport.DAL.Enum;

namespace UniTransport.PLL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminVehicleController : Controller
    {
        private readonly ITripService _tripService;
        private readonly IVehicleService _vehicleService;

        public AdminVehicleController(ITripService tripService, IVehicleService vehicleService)
        {
            _tripService = tripService;
            _vehicleService = vehicleService;
        }

        // GET: AdminVehicle
        public async Task<IActionResult> Index()
        {
            var vehicles = await _vehicleService.GetAllVehiclesAsync();
            return View(vehicles);
        }

        // GET: AdminVehicle/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vehicle vehicle)
        {
            if (!ModelState.IsValid)
                return View(vehicle);

            try
            {
                await _vehicleService.AddVehicleAsync(vehicle);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Failed to create vehicle: " + ex.Message);
                return View(vehicle);
            }
        }

        // GET: AdminVehicle/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
            if (vehicle == null)
                return NotFound();

            return View(vehicle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Vehicle vehicle)
        {
            if (!ModelState.IsValid)
                return View(vehicle);

            try
            {
                await _vehicleService.UpdateVehicleAsync(vehicle);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Failed to update vehicle: " + ex.Message);
                return View(vehicle);
            }
        }

        // POST: AdminVehicle/ToggleStatus/5
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
            if (vehicle == null)
                return NotFound();

            vehicle.IsActive = !vehicle.IsActive;
            await _vehicleService.UpdateVehicleAsync(vehicle);
            return RedirectToAction(nameof(Index));
        }

        // GET: AdminVehicle/VehicleTrips/5
        public async Task<IActionResult> VehicleTrips(int id)
        {
            var trips = await _tripService.GetTripsByVehicleIdAsync(id);
            return View(trips);
        }
    }
}
