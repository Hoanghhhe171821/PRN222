using Microsoft.EntityFrameworkCore;
using AssignmentPRN222.Interfaces;
using AssignmentPRN222.Models;

namespace AssignmentPRN222.Repository
{
    public class SeatBookingRepository : ISeatBooking
    {
        protected readonly ProjectPrn222Context _dbcontext;
        public SeatBookingRepository(ProjectPrn222Context dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public int FindSeatBookingId(int showTimeid, int seatId)
        {
            return _dbcontext.SeatsBookings.Where(x=>x.ShowTimeId==showTimeid && seatId==x.SeatId).Select(x=>x.Id).FirstOrDefault();
        }

        public List<SeatsBooking>  GetList(int id)
        {
            return _dbcontext.SeatsBookings.Include(x=>x.Seat).Include(x=>x.ShowTime).Where(x => x.ShowTimeId == id).ToList();

        }

        public SeatsBooking GetSeatBookingById(int id)
        {
            return _dbcontext.SeatsBookings.Include(x=>x.Seat).Include(x=>x.ShowTime).FirstOrDefault(x => x.Id == id);
        }
        public bool UpdateSeatBooking(SeatsBooking seatBooking) {
            if (seatBooking.IsBooked == false)
            {
                seatBooking.IsBooked = true;
                _dbcontext.SeatsBookings.Update(seatBooking);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
