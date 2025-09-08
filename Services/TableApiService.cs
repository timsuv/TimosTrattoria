using RestautantMvc.DTOs;
using System.Text;
using System.Text.Json;

namespace RestautantMvc.Services
{
    public interface ITableApiService
    {
        Task<IEnumerable<TableResponse>> GetAllTables();
        Task<TableResponse?> GetTableById(int id);
        Task<bool> DeleteTable(int id);
        Task<TableResponse?> CreateTable(CreateTableRequest request);
        Task<TableResponse?> UpdateTable(int id, UpdateTableRequest request);
    }
    public class TableApiService : ITableApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TableApiService(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = clientFactory.CreateClient("RestoApi");
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TableResponse?> CreateTable(CreateTableRequest request)
        {
            try
            {
                SetAuthHeader();

                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("tables", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    return JsonSerializer.Deserialize<TableResponse>(responseJson, options);
                }

                return null;


            }
            catch (Exception)
            {

                return null;
            }
        }

        public async Task<bool> DeleteTable(int id)
        {
            try
            {
                SetAuthHeader();
                var response = await _httpClient.DeleteAsync($"tables/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<IEnumerable<TableResponse>> GetAllTables()
        {
            try
            {
                var response = await _httpClient.GetAsync("Tables");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var tables = JsonSerializer.Deserialize<List<TableResponse>>(json, options) ?? new List<TableResponse>();
                    return tables;
                }
                return new List<TableResponse>();

            }
            catch (Exception)
            {

                return new List<TableResponse>();
            }
        }

        public async Task<TableResponse?> GetTableById(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"tables/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var table = JsonSerializer.Deserialize<TableResponse>(json, options);

                    return table;
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<TableResponse?> UpdateTable(int id, UpdateTableRequest request)
        {
            try
            {
                SetAuthHeader();

                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"tables/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    return JsonSerializer.Deserialize<TableResponse>(responseJson, options);
                }

                return null;
            }
            catch (Exception)
            {
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
    }
}
