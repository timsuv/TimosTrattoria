using Microsoft.AspNetCore.Mvc;
using RestautantMvc.Services;

namespace RestautantMvc.ViewModels
{
    public class TablesViewComponent : ViewComponent
    {
        private readonly ITableApiService _tableApiService;
        public TablesViewComponent(ITableApiService tableApiService)
        {
            _tableApiService = tableApiService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var tables = await _tableApiService.GetAllTables();

            var model = new TablesViewModel
            {
                Tables = tables.OrderBy(t=> t.TableNumber).ToList()
            };

            return View(model);
        }
    }
}
