using Microsoft.AspNetCore.Mvc;

namespace RestautantMvc.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
