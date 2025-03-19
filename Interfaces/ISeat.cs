using AssignmentPRN222.Models;

namespace AssignmentPRN222.Interfaces
{
    public interface ISeat
    {
        void Create(Seat seat);

        List<Seat> GetSeatList(int roomId);

        int FindSeatId(int col, int rol, int RoomId);
    }
}
