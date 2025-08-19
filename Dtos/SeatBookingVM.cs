namespace AssignmentPRN222.Dtos
{
    public class SeatBookingVM
    {
        public int SeatRow { get; set; }
        public int SeatClo { get; set; }
        public string SeatType { get; set; }
        public string RoomName { get; set; }
        public string CinemaName { get; set; }
        public string ProvinceName { get; set; }
        public string MovieName { get; set; }
        public DateOnly DateShowTime { get; set; }
        public TimeSpan StartTime { get; set; }
        public int TicketNo { get; set; }
    }
}
