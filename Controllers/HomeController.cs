using Microsoft.AspNetCore.Mvc;

namespace RestautantMvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(); 
        }
    }
}
