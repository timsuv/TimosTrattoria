using RestautantMvc.DTOs;
using RestautantMvc.Models;
using RestautantMvc.Services;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Text;
using System.Text.Json;

namespace RestautantMvc.Services
{
    public interface IMenuApiService
    {
        Task<IEnumerable<MenuItemResponse>> GetAllMenuItems();
        Task<MenuItemResponse?> GetMenuItemById(int id);
        Task<MenuItemResponse?> UpdateMenuItem(int id, UpdateMenuItem request);
    }
}


public class MenuApiService : IMenuApiService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public MenuApiService(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = clientFactory.CreateClient("RestoApi");
        _httpContextAccessor = httpContextAccessor;
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

    public async Task<MenuItemResponse?> GetMenuItemById(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"Menu/{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                return JsonSerializer.Deserialize<MenuItemResponse>(json, options);
            }
            return null;
        }
        catch (Exception)
        {

            return null;
        }
    }
    public async Task<MenuItemResponse?> UpdateMenuItem(int id, UpdateMenuItem request)
    {
        try
        {
            SetAuthHeader();
            var jsonObject = new
            {
                name = request.Name,
                price = request.Price,
                description = request.Description,
                category = (int)request.Category,  // Force it to be an integer
                isPopular = request.IsPopular,
                bildUrl = request.BildUrl
            };

            var json = JsonSerializer.Serialize(jsonObject);
            Console.WriteLine($"Sending to API: {json}");
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"Menu/{id}", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Error: {errorContent}");
            }
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<MenuItemResponse>(responseJson, options);
            }

            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            return null;
        }
    }

    private void SetAuthHeader()
    {
        var token = _httpContextAccessor.HttpContext?.Session.GetString("AuthToken");
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
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

