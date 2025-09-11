using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using RestautantMvc.DTOs;
using RestautantMvc.Services;
using RestautantMvc.ViewModels;

namespace RestautantMvc.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (_authService.IsAuth().Result)
            {
                return RedirectToLocal(returnUrl) ?? RedirectToAction("Index", "Admin");
            }
            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var loginRequest = new AdminLoginRequest
            {
                Username = model.Username,
                Password = model.Password
            };

            var result = await _authService.Login(loginRequest);
            if (result != null)
            {
                TempData["SuccessMessage"] = $"Welcome back, {result.Username}!";
                return RedirectToLocal(model.ReturnUrl) ?? RedirectToAction("Index", "Admin");
            }

            ModelState.AddModelError(string.Empty, "Invalide username or password.");
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(!ModelState.IsValid)
                return View(model);

            var secret = _configuration["AdminRegistration:Code"];
            if(model.SecretCode != secret)
            {
                ModelState.AddModelError("", "Invalid secret code. Ask the management for the code");
                return View(model);
            }

            var registerRequest = new AdminRegisterRequest
            {
                Username = model.Username,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword
            };

            var result = await _authService.Register(registerRequest);
            if (result != null)
            {
                TempData["SuccessMessage"] = "Admin registered successfully! Please login.";
                return RedirectToAction("Login");
            }

            ModelState.AddModelError("", "Registration failed. Try again.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();
            TempData["InfoMessage"] = "You have been logged out successfully.";
            return RedirectToAction("Index", "Home");

        }


        private IActionResult? RedirectToLocal(string? returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return null;
        }
    }
}
