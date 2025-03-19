using Microsoft.EntityFrameworkCore;
using AssignmentPRN222.Interfaces;
using AssignmentPRN222.Models;

namespace AssignmentPRN222.Repository
{
    public class RoomRepository : IRoom
    {
        protected readonly ProjectPrn222Context _dbcontext;
        public RoomRepository(ProjectPrn222Context dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public void CreateRom(Room room)
        {
            List<Seat> seats = new List<Seat>();
            for (int i = 1; i <= room.TotalRow; i++)
            {
                for (int j = 1; j <= room.TotalColum; j++)
                {
                    string typeSeat = (room.TotalRow <= 2 || i <= 2) ? "A" : "B";
                    seats.Add(new Seat
                    {
                        RowNumber = i,
                        CloNumber = j,
                        TypeSeat = typeSeat,
                        RoomId = room.RoomId
                    });
                }
            }
            room.Seats = seats;
            _dbcontext.Rooms.Add(room);
        }

        public void DeleteSoft(int roomId)
        {
            var room= _dbcontext.Rooms.FirstOrDefault(r => r.RoomId==roomId);
            room.IsDelete=true;
            _dbcontext.Update(room);
        }

        public List<Room> GetAll()
        {
            return _dbcontext.Rooms.Include(x=>x.Cinema).Where(x=>!x.IsDelete).ToList();
        }

        public List<Room> GetRoomByCinemaId(int cinemaId)
        {
            return _dbcontext.Rooms.Where(x=> x.CinemaId == cinemaId && !x.IsDelete).ToList();
        }

        public Room GetRoomById(int roomId)
        {
            Room x= _dbcontext.Rooms.FirstOrDefault(x => x.RoomId == roomId);
            return x;
        }

        public void Update(Room room)
        {
            _dbcontext.Update(room);
        }
    }
}
