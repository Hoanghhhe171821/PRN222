using Microsoft.EntityFrameworkCore;
using AssignmentPRN222.Interfaces;
using AssignmentPRN222.Models;

namespace AssignmentPRN222.Repository
{
    public class MovieRepository : IMovie
    {
        protected readonly ProjectPrn222Context _dbcontext;
        public MovieRepository(ProjectPrn222Context dbcontext)
        {
            _dbcontext = dbcontext;
        }
        

        public void createMovice(Movie movie)
        {
            _dbcontext.Movies.Add(movie);
        }

        public void DeleteSoft(int id)
        {
            var movie= _dbcontext.Movies.SingleOrDefault(x => x.Id == id);
            movie.IsDelete = true;
            _dbcontext.Update(movie);
        }

        public List<int> getMovieByDayandCinema(DateOnly date, int cinemaid)
        {
            var listMovie=_dbcontext.ShowTimes.Where(x=>x.DateShowTime==date&&x.CinemaId==cinemaid  ).Select(p=>p.MovieId).Distinct().ToList();
            return listMovie;
        }

        public Movie getMovieById(int id)
        {
            return _dbcontext.Movies.Find(id);
        }

        public List<Movie> getMovieList(string text)
        {
           return  _dbcontext.Movies.Where(x=>!x.IsDelete && x.Name.Contains(text)).ToList();
        }

        public void Update(Movie movie)
        {
            _dbcontext.Update(movie);
        }

    }
}
