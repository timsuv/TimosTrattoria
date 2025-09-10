using Microsoft.AspNetCore.Mvc;
using RestautantMvc.Services;
using RestautantMvc.ViewModels;

namespace RestautantMvc.ViewComponents
{
    public class BookingsViewComponent : ViewComponent
    {
        private readonly IBookingApiServices _bookingApiServices;
        private readonly ITableApiService _tableApiService;
        public BookingsViewComponent(IBookingApiServices bookingApiServices, ITableApiService tableApiService)
        {
            _bookingApiServices = bookingApiServices;
            _tableApiService = tableApiService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var bookings = await _bookingApiServices.GetAllBookings();
            var tables = await _tableApiService.GetAllTables();

            var model = new BookingsViewModel
            {
                Bookings = bookings
                .Where(b => b.BookingDate >= DateTime.UtcNow)
                .OrderBy(b => b.BookingDate)
                .ToList(),
                Tables = tables.OrderBy(t=>t.TableNumber).ToList()
            };

            return View(model); 
        }

    }
}
