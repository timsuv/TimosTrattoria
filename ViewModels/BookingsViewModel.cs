using RestautantMvc.DTOs;

namespace RestautantMvc.ViewModels
{
    public class BookingsViewModel
    {
        public List<BookingResponse> Bookings { get; set; } = new();
        public List<TableResponse> Tables { get; set; } = new();

    }
}
