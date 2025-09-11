namespace RestautantMvc.DTOs.BookingDTOs
{
    public class CreateBooking
    {
        public DateTime BookingDate { get; set; }
        public TimeSpan BookingTime { get; set; }
        public int NumberOfGuests { get; set; }
        public int TableId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
    }
}
