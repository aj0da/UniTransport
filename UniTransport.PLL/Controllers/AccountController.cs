using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Claims;
using UniTransport.DAL.Database;
using UniTransport.DAL.Entities;
using UniTransport.BLL.ModelVM.StudentVM;

namespace UniTransport.PLL.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        //private readonly Clinic_System.BLL.Service.Abstraction.IEmailSender emailSender;
        private readonly ApplicationDbContext _db;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager/*, Clinic_System.BLL.Service.Abstraction.IEmailSender emailSender*/, ApplicationDbContext _db, ILogger<AccountController> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            //this.emailSender = emailSender;
            this._db = _db;
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        public string GetLoggedInUserId()
        {
            return User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }


        [HttpGet]
        public async Task<IActionResult> Registration()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registration(StudentRegistrationVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
            };

            try
            {
                var existingUser = await userManager.FindByEmailAsync(model.Email);
                if (existingUser is not null)
                {
                    ModelState.AddModelError("", "An account with this email already exists. Please try a different email address.");
                    return View(model);
                }

                var result = await userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }

                // Add user to Student role
                await userManager.AddToRoleAsync(user, "Student");

                var student = new Student
                {
                    User = user,
                    StudentId = model.StudentId,
                    Bookings = new List<Booking>(),
                    RequestedTrips = new List<RequestedTrip>()
                };

                _db.Students.Add(student);
                await _db.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration");
                ModelState.AddModelError("", "An unexpected error occurred during registration. Please try again later.");
                return View(model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(StudentLoginVM model, string? returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var user = await userManager.FindByNameAsync(model.UserName);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid username or password. Please try again.");
                    return View(model);
                }

                var result = await userManager.CheckPasswordAsync(user, model.Password);
                if (!result)
                {
                    ModelState.AddModelError("", "Invalid username or password. Please try again.");
                    return View(model);
                }

                var claims = new List<Claim>();
                if (!string.IsNullOrEmpty(user.Image))
                {
                    claims.Add(new Claim("UserImage", user.Image));
                }

                await signInManager.SignInWithClaimsAsync(user, model.RememberMe, claims);

                var roles = await userManager.GetRolesAsync(user);
                if (roles.Contains("Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user login");
                ModelState.AddModelError("", "An unexpected error occurred during login. Please try again later.");
                return View(model);
            }
        }


        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        //    [HttpGet]
        //    public async Task<IActionResult> ChangePassword()
        //    {
        //        return View();
        //    }
        //    [HttpPost]
        //    public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        //    {
        //        try
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                var userId = GetLoggedInUserId();
        //                var user = await userManager.FindByIdAsync(userId);

        //                var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

        //                if (result.Succeeded)
        //                {
        //                    return RedirectToAction("Login", "Account");
        //                }
        //                else
        //                {
        //                    ModelState.AddModelError("", "Invalid UserName Or Password");

        //                }
        //                return View(model);
        //            }
        //        }
        //        catch (Exception)
        //        {

        //            return View(model);
        //        }

        //        return View(model);
        //    }

        //    public async Task<IActionResult> ConfirmEmail(string userId, string token)
        //    {
        //        if (userId == null || token == null)
        //        {
        //            return View("Error");
        //        }

        //        var user = await userManager.FindByIdAsync(userId);
        //        if (user == null)
        //        {
        //            return View("Error");
        //        }

        //        var result = await userManager.ConfirmEmailAsync(user, token);
        //        if (result.Succeeded)
        //        {
        //            return RedirectToAction("Login", "Account");
        //        }

        //        return View("Error");
        //    }


        //    [AllowAnonymous]
        //    public IActionResult ExternalLogin(string provider, string returnUrl = null)
        //    {
        //        var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
        //        var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        //        return Challenge(properties, provider);
        //    }

        //    [AllowAnonymous]
        //    public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        //    {
        //        if (remoteError != null)
        //        {
        //            ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
        //            return RedirectToAction(nameof(Login));
        //        }

        //        var info = await signInManager.GetExternalLoginInfoAsync();
        //        if (info == null)
        //        {
        //            return RedirectToAction(nameof(Login));
        //        }

        //        // Attempt to sign in the user with this external login provider
        //        var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
        //        if (result.Succeeded)
        //        {
        //            // Successful sign-in
        //            return RedirectToAction("Index", "Home");
        //        }
        //        else
        //        {
        //            // If the user does not have an account, create one
        //            ViewData["ReturnUrl"] = returnUrl;
        //            ViewData["LoginProvider"] = info.LoginProvider;
        //            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        //            return View("Registerion", new RegisterionVM { Email = email });
        //        }
        //    }
        //    [HttpGet]

        //    public IActionResult ForgetPassword()
        //    {
        //        return View(new ForgetPasswordMV());
        //    }

        //    [HttpPost]
        //    public async Task<IActionResult> ForgetPassword(ForgetPasswordMV model)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var user = await userManager.FindByEmailAsync(model.Email);
        //            if (user != null)
        //            {
        //                var token = await userManager.GeneratePasswordResetTokenAsync(user);
        //                var resetLink = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, protocol: Request.Scheme);

        //                var subject = "Reset Password";
        //                var message = $"Please reset your password by clicking this link: <a href='{resetLink}'>Reset Password</a>";
        //                await emailSender.SendEmailAsync(user.Email, subject, message);
        //            }

        //            // Display a view indicating that a reset link has been sent
        //            model.EmailSent = true;
        //            return View(model);
        //        }
        //        return View(model);
        //    }

        //    [HttpGet]
        //    public IActionResult ResetPassword(string token, string email)
        //    {
        //        if (token == null || email == null)
        //        {
        //            return View("Error");
        //        }
        //        var model = new ResetPasswordVM { Token = token, Email = email };
        //        return View(model);
        //    }

        //    [HttpPost]
        //    public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return View(model);
        //        }

        //        var user = await userManager.FindByEmailAsync(model.Email);
        //        if (user == null)
        //        {
        //            ModelState.AddModelError(string.Empty, "Invalid request");
        //            return View(model);
        //        }

        //        var result = await userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
        //        if (result.Succeeded)
        //        {
        //            return RedirectToAction("Login", "Account");
        //        }

        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError(string.Empty, error.Description);
        //        }
        //        return View(model);
    }
    }
