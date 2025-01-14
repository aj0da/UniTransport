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
            if (ModelState.IsValid) 
            {
                if (_requestedTripService.CreatRequestedTrip(requestNowVM, GetLoggedInUserId()))
                {
                    return RedirectToAction("Profile", "Student");
                }
                return RedirectToAction("Index", "Home");
            }
            return View(requestNowVM);
        }
    }
}
