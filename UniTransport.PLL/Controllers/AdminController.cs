using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniTransport.DAL.Database;
using UniTransport.DAL.Entities;

namespace UniTransport.PLL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminController(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users
                .Select(u => new UserViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    PhoneNumber = u.PhoneNumber,
                    Roles = _userManager.GetRolesAsync(u).Result
                })
                .ToListAsync();

            return PartialView(users);
        }

        public async Task<IActionResult> Vehicles()
        {
            var vehicles = await _context.Vehicles.ToListAsync();
            return PartialView(vehicles);
        }

        public async Task<IActionResult> Schedules()
        {
            var trips = await _context.Trips
                .Include(t => t.Vehicle)
                .OrderBy(t => t.DepartureTime)
                .ToListAsync();
            return PartialView(trips);
        }

        public async Task<IActionResult> Bookings()
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

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Don't allow deleting the admin user
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return BadRequest("Cannot delete admin user");
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Users));
            }

            ModelState.AddModelError("", "Failed to delete user");
            return RedirectToAction(nameof(Users));
        }
    }

    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public IList<string> Roles { get; set; }
    }
}
