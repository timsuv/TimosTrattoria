using Microsoft.AspNetCore.Mvc;
using RestautantMvc.Models;
using System.Diagnostics;

namespace RestautantMvc.Controllers
{
    
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 401:
                    return View("Error401");
                case 403:
                    return View("Error403");
                default:
                    return View("Error");
            }
        }
        [Route("Error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
