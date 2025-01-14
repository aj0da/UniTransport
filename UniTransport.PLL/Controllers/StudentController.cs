using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UniTransport.BLL.Service.Interface;

namespace UniTransport.PLL.Controllers
{
    public class StudentController : Controller
    {
		private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        private string GetLoggedInUserId()
		{
			return User?.FindFirstValue(ClaimTypes.NameIdentifier);
		}

        [Authorize(Roles ="Student")]
        public IActionResult Profile()
		{
            var studentProfile = _studentService.GetStudentProfile(GetLoggedInUserId()); 
			return View(studentProfile);
		}

	}
}
