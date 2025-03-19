using Microsoft.EntityFrameworkCore;
using AssignmentPRN222.Interfaces;
using AssignmentPRN222.Models;

namespace AssignmentPRN222.Repository
{
    public class SeatRepository : ISeat
    {
        protected readonly ProjectPrn222Context _dbcontext;
        public SeatRepository(ProjectPrn222Context dbcontext)
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
