using Microsoft.EntityFrameworkCore;
using Project_NH.Interfaces;
using Project_NH.Models;

namespace Project_NH.Repository
{
    public class SeatBookingRepository : ISeatBooking
    {
        protected readonly AppicationDbcontext _dbcontext;
        public SeatBookingRepository(AppicationDbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public int FindSeatBookingId(int showTimeid, int seatId)
        {
            return _dbcontext.SeatsBooking.Where(x=>x.ShowTimeId==showTimeid && seatId==x.SeatId).Select(x=>x.Id).FirstOrDefault();
        }

        public List<SeatBooking>  GetList(int id)
        {
            return _dbcontext.SeatsBooking.Include(x=>x.Seat).Include(x=>x.ShowTime).Where(x => x.ShowTimeId == id).ToList();

        }

        public SeatBooking GetSeatBookingById(int id)
        {
            return _dbcontext.SeatsBooking.Include(x=>x.Seat).Include(x=>x.ShowTime).FirstOrDefault(x => x.Id == id);
        }
        public bool UpdateSeatBooking(SeatBooking seatBooking) {
            if (seatBooking.IsBooked == false)
            {
                seatBooking.IsBooked = true;
                _dbcontext.SeatsBooking.Update(seatBooking);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
