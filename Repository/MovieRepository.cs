using Microsoft.EntityFrameworkCore;
using Project_NH.Interfaces;
using Project_NH.Models;

namespace Project_NH.Repository
{
    public class MovieRepository : IMovie
    {
        protected readonly AppicationDbcontext _dbcontext;
        public MovieRepository(AppicationDbcontext dbcontext)
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
            var listMovie=_dbcontext.ShowTime.Where(x=>x.DateShowTime==date&&x.CinemaId==cinemaid  ).Select(p=>p.MovieId).Distinct().ToList();
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
