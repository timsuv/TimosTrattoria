using Microsoft.AspNetCore.Mvc;
using RestautantMvc.DTOs.MenuDTOs;
using RestautantMvc.Models;
using RestautantMvc.Services;

namespace RestautantMvc.Controllers
{
    public class MenuController : Controller
    {
        private readonly IMenuApiService _menuApiService;

        public MenuController(IMenuApiService menuApiService)
        {
            _menuApiService = menuApiService;
        }


        public async Task<IActionResult> Index()
        {
            try
            {
                var allMenuItems = await _menuApiService.GetAllMenuItems();
                return View(allMenuItems); 
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "Unable to load menu at this time.";
                return View(new List<MenuItemResponse>());
            }
        }

    }
}
