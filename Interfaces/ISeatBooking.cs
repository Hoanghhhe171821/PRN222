using AssignmentPRN222.Models;

namespace AssignmentPRN222.Interfaces
{
    public interface ISeatBooking
    {
        Task<List<SeatsBooking>> GetList(int id);
        int FindSeatBookingId(int showTimeid, int seatId);
       SeatsBooking GetSeatBookingById(int id);
       bool UpdateSeatBooking(SeatsBooking seatBooking);


    }
}
