using Microsoft.AspNetCore.Mvc;
using RestautantMvc.Services;

namespace RestautantMvc.Controllers
{
    public class AdminController : Controller
    {
        private readonly IBookingApiServices _bookingApiServices;
        private readonly ITableApiService _tableApiService;
        private readonly IAuthService _authService;
        public AdminController(IBookingApiServices bookingApiServices, ITableApiService tableApiService, IAuthService authService)
        {
            _authService = authService;
            _bookingApiServices = bookingApiServices;
            _tableApiService = tableApiService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
