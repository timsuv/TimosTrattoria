using RestautantMvc.DTOs.BookingDTOs;
using RestautantMvc.DTOs.TableDTOs;

namespace RestautantMvc.ViewModels
{
    public class AdminDashboardViewModel
    {
        public List<BookingResponse> Bookings { get; set; } = new();
        public List<TableResponse> Tables { get; set; } = new();
        public string ActiveTab { get; set; } = "bookings";

        public int TotalBookings => Bookings.Count;
        public int TotalTables => Tables.Count;
    }
}
