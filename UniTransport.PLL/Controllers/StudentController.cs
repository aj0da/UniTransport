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
            try
            {
                var userId = GetLoggedInUserId();
                var studentProfile = _studentService.GetStudentProfile(userId);
                if (studentProfile == null)
                {
                    return NotFound("Student profile not found");
                }
                return View(studentProfile);
            }
            catch (InvalidOperationException)
            {
                return RedirectToAction("Login", "Account");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }

}
