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
        public AuthController(IAuthService authService)
        {
            _authService = authService;
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
