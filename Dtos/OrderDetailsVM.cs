namespace AssignmentPRN222.Dtos
{
    public class OrderDetailsVM
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public int PriceFrist { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public List<SeatBookingVM> Seats { get; set; }
    }
}
