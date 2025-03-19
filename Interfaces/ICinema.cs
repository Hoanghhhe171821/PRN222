using AssignmentPRN222.Models;

namespace AssignmentPRN222.Interfaces
{
    public interface ICinema
    {
        List<Cinema> getCinemaByIdProvince(int provinceId);
        List<Cinema> getAllCinema(int provinceId,string text);
        bool Create(Cinema cinema);
        Cinema getCinema(int id);
        void Update(Cinema cinema);
        void DeleteSoft(int id);
    }
}
