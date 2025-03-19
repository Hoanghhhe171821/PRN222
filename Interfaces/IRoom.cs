using AssignmentPRN222.Models;

namespace AssignmentPRN222.Interfaces
{
    public interface IRoom
    {
        List<Room> GetAll();
        void CreateRom(Room room);
        List<Room> GetRoomByCinemaId(int cinemaId);
        Room GetRoomById(int roomId);
        void Update(Room room);
        void DeleteSoft(int roomId);
        
    }
}
