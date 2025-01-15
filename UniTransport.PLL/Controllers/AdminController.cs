using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniTransport.BLL.ModelVM.TripVM;
using UniTransport.BLL.ModelVM.UserVM;
using UniTransport.DAL.Database;
using UniTransport.DAL.Entities;
using UniTransport.DAL.Enum;

namespace UniTransport.PLL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminController> _logger;

        public AdminController(UserManager<User> userManager, ApplicationDbContext context, ILogger<AdminController> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Users()
        {
            try
            {
                var users = await _userManager.Users
                    .Select(u => new UserViewModel
                    {
                        Id = u.Id ?? string.Empty,
                        UserName = u.UserName ?? string.Empty,
                        Email = u.Email ?? string.Empty,
                        PhoneNumber = u.PhoneNumber ?? string.Empty,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Image = u.Image ?? string.Empty,
                        Roles = _userManager.GetRolesAsync(u).Result.ToList()
                    }).ToListAsync();

                return View(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users");
                ModelState.AddModelError("", "An error occurred while retrieving users");
                return View(new List<UserViewModel>());
            }
        }

        public async Task<IActionResult> Vehicles()
        {
            try
            {
                var vehicles = await _context.Vehicles.ToListAsync();
                return PartialView(vehicles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vehicles");
                ModelState.AddModelError("", "An error occurred while retrieving vehicles");
                return PartialView(new List<Vehicle>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddVehicle([FromBody] Vehicle vehicle)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return Json(new { success = false, message = string.Join(", ", errors) });
                }

                // Check if license plate already exists
                if (await _context.Vehicles.AnyAsync(v => v.LicensePlate == vehicle.LicensePlate))
                {
                    return Json(new { success = false, message = "A vehicle with this license plate already exists" });
                }

                // Initialize collections
                vehicle.Trips = new List<Trip>();
                vehicle.IsActive = true;

                _context.Vehicles.Add(vehicle);
                await _context.SaveChangesAsync();

                // Get the enum name for vehicle type
                string vehicleTypeName = Enum.GetName(typeof(VehicleType), vehicle.VehicleType) ?? "Unknown";

                return Json(new
                {
                    success = true,
                    message = "Vehicle added successfully",
                    vehicle = new
                    {
                        vehicleId = vehicle.VehicleId,
                        licensePlate = vehicle.LicensePlate,
                        vehicleType = vehicle.VehicleType,
                        vehicleTypeName = vehicleTypeName,
                        capacity = vehicle.Capacity,
                        isActive = vehicle.IsActive
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding vehicle with license plate {LicensePlate}", vehicle.LicensePlate);
                return Json(new { success = false, message = "An error occurred while adding the vehicle" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            try
            {
                var vehicle = await _context.Vehicles
                    .Include(v => v.Trips.Where(t => t.DepartureTime > DateTime.Now))
                    .FirstOrDefaultAsync(v => v.VehicleId == id);

                if (vehicle == null)
                {
                    return Json(new { success = false, message = "Vehicle not found" });
                }

                // Check if vehicle has any upcoming trips
                if (vehicle.Trips != null && vehicle.Trips.Any())
                {
                    return Json(new { success = false, message = "Cannot delete vehicle as it has upcoming trips scheduled" });
                }

                // Check if vehicle has any past trips
                var hasPastTrips = await _context.Trips
                    .AnyAsync(t => t.VehicleId == id && t.DepartureTime <= DateTime.Now);

                if (hasPastTrips)
                {
                    // Instead of deleting, mark as inactive
                    vehicle.IsActive = false;
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Vehicle has been marked as inactive due to having past trips" });
                }

                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting vehicle {VehicleId}", id);
                return Json(new { success = false, message = "An error occurred while deleting the vehicle" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVehicle([FromBody] Vehicle vehicle)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return Json(new { success = false, message = string.Join(", ", errors) });
                }

                var existingVehicle = await _context.Vehicles
                    .Include(v => v.Trips.Where(t => t.DepartureTime > DateTime.Now))
                    .FirstOrDefaultAsync(v => v.VehicleId == vehicle.VehicleId);

                if (existingVehicle == null)
                {
                    return Json(new { success = false, message = "Vehicle not found" });
                }

                // Check if license plate is already used by another vehicle
                var duplicateLicense = await _context.Vehicles
                    .AnyAsync(v => v.LicensePlate == vehicle.LicensePlate &&
                                  v.VehicleId != vehicle.VehicleId);

                if (duplicateLicense)
                {
                    return Json(new { success = false, message = "A vehicle with this license plate already exists" });
                }

                // Validate capacity changes if there are upcoming trips
                if (vehicle.Capacity < existingVehicle.Capacity && existingVehicle.Trips.Any())
                {
                    // Check if any upcoming trips have more bookings than the new capacity
                    var maxBookings = await _context.Trips
                        .Where(t => t.VehicleId == vehicle.VehicleId && t.DepartureTime > DateTime.Now)
                        .Select(t => t.Bookings.Count)
                        .MaxAsync();

                    if (maxBookings > vehicle.Capacity)
                    {
                        return Json(new { success = false, message = $"Cannot reduce capacity below current bookings. Maximum bookings on upcoming trips: {maxBookings}" });
                    }
                }

                // Check if vehicle can be deactivated
                if (!vehicle.IsActive && existingVehicle.IsActive && existingVehicle.Trips.Any())
                {
                    return Json(new { success = false, message = "Cannot deactivate vehicle with upcoming trips" });
                }

                // Update properties
                existingVehicle.LicensePlate = vehicle.LicensePlate;
                existingVehicle.VehicleType = vehicle.VehicleType;
                existingVehicle.Capacity = vehicle.Capacity;
                existingVehicle.IsActive = vehicle.IsActive;

                await _context.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    vehicle = new
                    {
                        vehicleId = existingVehicle.VehicleId,
                        licensePlate = existingVehicle.LicensePlate,
                        vehicleType = existingVehicle.VehicleType,
                        capacity = existingVehicle.Capacity,
                        isActive = existingVehicle.IsActive
                    },
                    message = "Vehicle updated successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating vehicle {VehicleId}", vehicle.VehicleId);
                return Json(new { success = false, message = "An error occurred while updating the vehicle" });
            }
        }

        public async Task<IActionResult> Schedules()
        {
            try
            {
                var trips = await _context.Trips
                    .Include(t => t.Vehicle)
                    .OrderBy(t => t.DepartureTime)
                    .ToListAsync();
                return PartialView(trips);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving schedules");
                ModelState.AddModelError("", "An error occurred while retrieving schedules");
                return PartialView(new List<Trip>());
            }
        }

        public async Task<IActionResult> Bookings()
        {
            try
            {
                var bookings = await _context.Bookings
                    .Include(b => b.Student)
                        .ThenInclude(s => s.User)
                    .Include(b => b.Trip)
                        .ThenInclude(t => t.Vehicle)
                    .OrderByDescending(b => b.BookingTime)
                    .ToListAsync();
                return PartialView(bookings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving bookings");
                ModelState.AddModelError("", "An error occurred while retrieving bookings");
                return PartialView(new List<Booking>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest("Invalid user ID");
                }

                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                // Don't allow deleting the admin user
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    return BadRequest("Cannot delete admin user");
                }

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Failed to delete user");
                    return BadRequest(ModelState);
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user {UserId}", id);
                return StatusCode(500, "An error occurred while deleting the user");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetActiveVehicles()
        {
            try
            {
                var vehicles = await _context.Vehicles
                    .Where(v => v.IsActive)
                    .Select(v => new {
                        v.VehicleId,
                        v.LicensePlate,
                        v.VehicleType,
                        v.Capacity
                    })
                    .ToListAsync();

                return Json(new { success = true, vehicles });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active vehicles");
                return Json(new { success = false, message = "Error retrieving vehicles" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTrip(int tripId)
        {
            try
            {
                var trip = await _context.Trips
                    .Include(t => t.Vehicle)
                    .FirstOrDefaultAsync(t => t.TripId == tripId);

                if (trip == null)
                {
                    return Json(new { success = false, message = "Trip not found" });
                }

                return Json(new
                {
                    success = true,
                    trip = new
                    {
                        trip.TripId,
                        trip.VehicleId,
                        trip.DepartureLocation,
                        trip.ArrivalLocation,
                        trip.DepartureTime,
                        trip.ArrivalTime,
                        Price = (double)trip.Price,
                        trip.AvailableSeats,
                        trip.TripStatus
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving trip {TripId}", tripId);
                return Json(new { success = false, message = "Error retrieving trip details" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTrip(TripViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Invalid trip data" });
                }

                var vehicle = await _context.Vehicles
                    .Include(v => v.Trips)
                    .FirstOrDefaultAsync(v => v.VehicleId == model.VehicleId);

                if (vehicle == null)
                {
                    return Json(new { success = false, message = "Vehicle not found" });
                }

                // Check for overlapping trips
                if (HasOverlappingTrips(vehicle.Trips, model.DepartureTime, model.ArrivalTime))
                {
                    return Json(new { success = false, message = "Vehicle has overlapping trips during this time period" });
                }

                var trip = new Trip
                {
                    VehicleId = model.VehicleId,
                    DepartureLocation = model.DepartureLocation,
                    ArrivalLocation = model.ArrivalLocation,
                    DepartureTime = model.DepartureTime,
                    ArrivalTime = model.ArrivalTime,
                    Price = (double)model.Price,
                    AvailableSeats = vehicle.Capacity,
                    TripStatus = DAL.Enum.TripStatus.Active
                };

                _context.Trips.Add(trip);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Trip added successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding trip");
                return Json(new { success = false, message = "An error occurred while adding the trip" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTrip([FromBody] TripViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Invalid trip data" });
                }

                var trip = await _context.Trips
                    .Include(t => t.Vehicle)
                    .Include(t => t.Bookings)
                    .FirstOrDefaultAsync(t => t.TripId == model.TripId);

                if (trip == null)
                {
                    return Json(new { success = false, message = "Trip not found" });
                }

                var vehicle = await _context.Vehicles
                    .Include(v => v.Trips)
                    .FirstOrDefaultAsync(v => v.VehicleId == model.VehicleId);

                if (vehicle == null)
                {
                    return Json(new { success = false, message = "Vehicle not found" });
                }

                // Check for overlapping trips (excluding current trip)
                if (HasOverlappingTrips(vehicle.Trips.Where(t => t.TripId != trip.TripId), model.DepartureTime, model.ArrivalTime))
                {
                    return Json(new { success = false, message = "Vehicle has overlapping trips during this time period" });
                }

                // Update trip details
                trip.VehicleId = model.VehicleId;
                trip.DepartureLocation = model.DepartureLocation;
                trip.ArrivalLocation = model.ArrivalLocation;
                trip.DepartureTime = model.DepartureTime;
                trip.ArrivalTime = model.ArrivalTime;
                trip.Price = (double)model.Price;

                // Update available seats based on new vehicle capacity
                var bookedSeats = trip.Bookings?.Count ?? 0;
                trip.AvailableSeats = vehicle.Capacity - bookedSeats;

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Trip updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating trip");
                return Json(new { success = false, message = "An error occurred while updating the trip" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTrip(int tripId)
        {
            try
            {
                var trip = await _context.Trips
                    .Include(t => t.Bookings)
                    .FirstOrDefaultAsync(t => t.TripId == tripId);

                if (trip == null)
                {
                    return Json(new { success = false, message = "Trip not found" });
                }

                if (trip.Bookings?.Any() == true)
                {
                    return Json(new { success = false, message = "Cannot delete trip with existing bookings" });
                }

                if (trip.DepartureTime <= DateTime.Now)
                {
                    return Json(new { success = false, message = "Cannot delete past or ongoing trips" });
                }

                _context.Trips.Remove(trip);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Trip deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting trip {TripId}", tripId);
                return Json(new { success = false, message = "An error occurred while deleting the trip" });
            }
        }

        private bool HasOverlappingTrips(IEnumerable<Trip> trips, DateTime newStart, DateTime newEnd)
        {
            return trips.Any(t =>
                (newStart >= t.DepartureTime && newStart < t.ArrivalTime) ||
                (newEnd > t.DepartureTime && newEnd <= t.ArrivalTime) ||
                (newStart <= t.DepartureTime && newEnd >= t.ArrivalTime));
        }
    }
}
