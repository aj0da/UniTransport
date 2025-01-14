using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol.Core.Types;
using System.Security.Claims;
using UniTransport.BLL.ModelVM.TripVM;
using UniTransport.BLL.Service.Interface;
using UniTransport.DAL.Database;
using UniTransport.DAL.Entities;
using UniTransport.DAL.Repository.Interface;

namespace UniTransport.PLL.Controllers
{
	public class TripController : Controller
	{
		private readonly ITripService _tripService;
		public TripController(ITripService service)
		{
			_tripService = service;
		}
		private string GetLoggedInUserId()
		{
			return User?.FindFirstValue(ClaimTypes.NameIdentifier);
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public IActionResult Book()
		{
			var incomingTrips = _tripService.GetIncomingTrips();
			return View(incomingTrips);
		}
		[HttpPost]
		[Authorize(Roles ="Student")]
		public IActionResult Book(IEnumerable<BookTripsVM> trips)
		{
			if (trips.IsNullOrEmpty())
			{
				return BadRequest("No trips selected for booking.");
			}
			//trips has entites and not null
			if (_tripService.BookTrip(trips, GetLoggedInUserId()))
			{
				return RedirectToAction("Profile", "Student");
			}
			return RedirectToAction("Home", "Index");

		}
	}
}
