using RestautantMvc.DTOs.AuthDTOs;
using System.Text;
using System.Text.Json;

namespace RestautantMvc.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(AdminLoginRequest request);
        Task Logout();
        Task<bool> IsAuth();
        Task<string> GetCurrentUser();
        Task<AdminRegisterResponse?> Register(AdminRegisterRequest request);
    }
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthService(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = clientFactory.CreateClient("RestoApi");
            _httpContextAccessor = httpContextAccessor;
        }
        public Task<string> GetCurrentUser()
        {
            var username = _httpContextAccessor.HttpContext?.Session.GetString("Username");
            return Task.FromResult(username);
        }

        public Task<bool> IsAuth()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("AuthToken");
            return Task.FromResult(!string.IsNullOrEmpty(token));
        }

        public async Task<LoginResponse> Login(AdminLoginRequest request)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("Auth/login", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseJson, options);

                    if (loginResponse?.Token != null)
                    {
                        _httpContextAccessor.HttpContext?.Session.SetString("AuthToken", loginResponse.Token);
                        _httpContextAccessor.HttpContext?.Session.SetString("Username", loginResponse.Username ?? "");
                    }
                    return loginResponse;
                }
                return null;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public async Task<AdminRegisterResponse?> Register(AdminRegisterRequest request)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("Auth/register", content);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var responseJson = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var registerResponse = JsonSerializer.Deserialize<AdminRegisterResponse>(responseJson, options);
                return registerResponse;
            }
            catch (Exception)
            {
                return null;
            }
        }




        public Task Logout()
        {
            _httpContextAccessor.HttpContext?.Session.Clear();
            return Task.CompletedTask;
        }
    }
}
