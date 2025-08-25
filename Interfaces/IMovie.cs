using AssignmentPRN222.Models;
namespace AssignmentPRN222.Interfaces
{
    public interface IMovie
    {
        void createMovice(Movie movie);

        List<Movie> getMovieList(string text);
        List<Movie> getMovieListByAdmin(string text);
        Movie getMovieById(int id);
        List<int> getMovieByDayandCinema(DateOnly date, int cinemaid);
        void Update(Movie movie);
        void DeleteSoft(int id);
        void UnDeleteSoft(int id);
    }
}
