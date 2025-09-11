using RestautantMvc.DTOs.BookingDTOs;
using RestautantMvc.DTOs.TableDTOs;

namespace RestautantMvc.ViewModels
{
    public class BookingsViewModel
    {
        public List<BookingResponse> Bookings { get; set; } = new();
        public List<TableResponse> Tables { get; set; } = new();

    }
}
