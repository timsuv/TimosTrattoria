using Microsoft.AspNetCore.Mvc;
using RestautantMvc.Services;
using RestautantMvc.ViewModels;

namespace RestautantMvc.ViewComponents
{
    public class MenuItemsViewComponent : ViewComponent
    {
        private readonly IMenuApiService _menuApiService;
        public MenuItemsViewComponent(IMenuApiService menuApiService)
        {
            _menuApiService = menuApiService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _menuApiService.GetAllMenuItems();

            var model = new MenuViewModel
            {
                MenuItems = items.OrderBy(i=> i.Category).ThenBy(i=>i.Name).ToList()
            };

            return View(model);
        }
    }
}
