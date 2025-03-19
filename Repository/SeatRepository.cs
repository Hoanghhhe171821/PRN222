using Microsoft.EntityFrameworkCore;
using Project_NH.Interfaces;
using Project_NH.Models;

namespace Project_NH.Repository
{
    public class SeatRepository : ISeat
    {
        protected readonly AppicationDbcontext _dbcontext;
        public SeatRepository(AppicationDbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public void Create(Seat seat)
        {
            _dbcontext.Seats.Add(seat);
        }

        public int FindSeatId(int col, int row, int RoomId)
        {
            var x= _dbcontext.Seats.Where(x => x.RowNumber == row && x.CloNumber == col&&x.RoomId==RoomId).Select(x=>x.Id).FirstOrDefault();
            return x;
        }

        public List<Seat> GetSeatList(int roomId)
        {
            return _dbcontext.Seats.Where(x=>x.RoomId == roomId).ToList();
        }
    }
}
