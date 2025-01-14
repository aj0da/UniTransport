using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UniTransport.BLL.ModelVM.RequestedTripVM;
using UniTransport.BLL.Service.Interface;
using UniTransport.DAL.Repository.Interface;

namespace UniTransport.PLL.Controllers
{
	public class RequestedTripController : Controller
	{
        private readonly IRequestedTripService  _requestedTripService;

        public RequestedTripController(IRequestedTripService requestedTripService)
        {
            _requestedTripService = requestedTripService;
        }

        private string GetLoggedInUserId()
        {
            return User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [HttpGet]
        public IActionResult RequestNow()
        {
            return View();
        }
        [HttpPost]
		[Authorize(Roles = "Student")]
        public IActionResult RequestNow(RequestNowVM requestNowVM)
        {
            if (requestNowVM == null)
            {
                return BadRequest("Invalid request data");
            }

            if (!ModelState.IsValid)
            {
                return View(requestNowVM);
            }

            try
            {
                var userId = GetLoggedInUserId();
                if (_requestedTripService.CreatRequestedTrip(requestNowVM, userId))
                {
                    TempData["Success"] = "Trip request submitted successfully";
                    return RedirectToAction("Profile", "Student");
                }

                ModelState.AddModelError("", "Failed to create trip request. Please try again.");
                return View(requestNowVM);
            }
            catch (InvalidOperationException)
            {
                return RedirectToAction("Login", "Account");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while processing your request. Please try again.");
                return View(requestNowVM);
            }
        }
    }
}
