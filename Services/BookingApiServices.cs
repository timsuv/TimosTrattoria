using Microsoft.Extensions.Options;
using RestautantMvc.DTOs;
using System.Text;
using System.Text.Json;

namespace RestautantMvc.Services
{
    public interface IBookingApiServices
    {
        Task<IEnumerable<BookingResponse>> GetAllBookings();
        Task<BookingResponse?> GetBookingById(int id);
        Task<bool> DeleteBooking(int id);
        Task<bool> UpdateBookingTable(int bookingId, int newTableId);
    }
    public class BookingApiService : IBookingApiServices
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BookingApiService(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = clientFactory.CreateClient("RestoApi");
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> DeleteBooking(int id)
        {
            try
            {
                SetAuthHeader();
                var response = await _httpClient.DeleteAsync($"bookings/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {

                return false;
            }
        }

        private void SetAuthHeader()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("AuthToken");
            if(!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<IEnumerable<BookingResponse>> GetAllBookings()
        {
            try
            {
                SetAuthHeader();
                var response = await _httpClient.GetAsync("Bookings");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    return JsonSerializer.Deserialize<List<BookingResponse>>(json, options) ?? new List<BookingResponse>();
                }
                return new List<BookingResponse>();
            }
            catch (Exception)
            {

                return new List<BookingResponse>();
            }
        }

        public async Task<BookingResponse?> GetBookingById(int id)
        {
            try
            {
                SetAuthHeader();
                var response = await _httpClient.GetAsync($"bookings/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    return JsonSerializer.Deserialize<BookingResponse>(json, options);
                }
                return null;

            }
            catch (Exception)
            {

                return null;
            }
        }
        public async Task<bool> UpdateBookingTable(int bookingId, int newTableId)
        {
            try
            {
                SetAuthHeader();

                var currentBooking = await GetBookingById(bookingId);
                if (currentBooking == null)
                    return false;

                var updateRequest = new
                {
                    BookingDate = currentBooking.BookingDate,
                    BookingTime = currentBooking.BookingTime,
                    NumberOfGuests = currentBooking.NumberOfGuests,
                    TableId = newTableId

                };

                var json = JsonSerializer.Serialize(updateRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"bookings/{bookingId}", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {

                return false;
            }


        }
    }
}
