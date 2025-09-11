namespace RestautantMvc.DTOs.BookingDTOs
{
    public class UpdateBooking
    {
        public DateTime BookingDate { get; set; }
        public TimeSpan BookingTime { get; set; }
        public int NumberOfGuests { get; set; }
        public int TableId { get; set; }
        public int CustomerId { get; set; }
    }
}
