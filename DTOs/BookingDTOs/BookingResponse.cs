namespace RestautantMvc.DTOs.BookingDTOs
{
    public class BookingResponse
    {
        public int BookingId { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan BookingTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int NumberOfGuests { get; set; }
        public DateTime CreatedAt { get; set; }

        //customer
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }=string.Empty;
        public string CustomerEmail{ get; set; }=string.Empty;
        public string CustomerPhone { get; set; }=string.Empty;

        //table
        public int TableId { get; set; }
        public int TableNumber { get; set; }
        public int TableCapacity { get; set; }

    }
}
