using RestautantMvc.DTOs;
using RestautantMvc.Models;
using RestautantMvc.Services;
using System.ComponentModel;
using System.Text.Json;

namespace RestautantMvc.Services
{
    public interface IMenuApiService
    {
        Task<IEnumerable<MenuItemResponse>> GetAllMenuItems();
    }
}


    public class MenuApiService : IMenuApiService
    {
        private readonly HttpClient _httpClient;
        public MenuApiService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("RestoApi");
        }


        public async Task<IEnumerable<MenuItemResponse>> GetAllMenuItems()
        {
            try
            {
                var response = await _httpClient.GetAsync("Menu");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var items = JsonSerializer.Deserialize<List<MenuItemResponse>>(json, options) ?? new List<MenuItemResponse>();

                foreach (var item in items)
                {
                    item.CategoryName = GetCategoryDisplayName(item.Category);
                }
                return items;
            }
            return new List<MenuItemResponse>();

        }
            catch (Exception)
            {

                return new List<MenuItemResponse>();
            }
        }

        private static string GetCategoryDisplayName(MenuCategory category)
        {
            var field = category.GetType().GetField(category.ToString());
            var attribute = field?.GetCustomAttributes(typeof(DescriptionAttribute), false)
                                 .FirstOrDefault() as DescriptionAttribute;
            return attribute?.Description ?? category.ToString();
        }
    }

