using Microsoft.EntityFrameworkCore;
using AssignmentPRN222.Interfaces;
using AssignmentPRN222.Models;

namespace AssignmentPRN222.Repository
{
    public class ShowTimeRepository : IShowTime
    {
        protected readonly ProjectPrn222Context _dbcontext;
        public ShowTimeRepository(ProjectPrn222Context dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public void Create(ShowTime showTime)
        {
            _dbcontext.ShowTime.Add(showTime);
        }

        public void DeleteShowTime(int showTimeId)
        {
            var showTime=_dbcontext.ShowTime.Find(showTimeId);
            _dbcontext.ShowTime.Remove(showTime);
        }

        public List<ShowTime> GetAll(int movieId,int cinemaId,int roomId)
        {
            return _dbcontext.ShowTime.Where(x=>(x.MovieId==movieId||movieId==0)&&(x.CinemaId==cinemaId||cinemaId==0)&&(x.RoomId==roomId||roomId==0))
                .Include(x => x.Movie).OrderByDescending(x=>x.DateShowTime).ToList();
        }

        public List<ShowTime> GetShowTimeByDateandCinemaandMovie(DateOnly date, int cinemaid, int movieId)
        {
            TimeSpan timeSpan = DateTime.Now.TimeOfDay;
            var query = _dbcontext.ShowTime
                .Where(x => x.DateShowTime == date && x.CinemaId == cinemaid && x.MovieId == movieId);
            if (date == DateOnly.FromDateTime(DateTime.Today))
            {
                query = query.Where(x => x.StartTime > timeSpan);
            }
            query = query.OrderBy(x => x.StartTime);
            return query.ToList();
            return null;
        }


        public ShowTime GetShowTimeById(int id)
        {
            return _dbcontext.ShowTime
                .Include(x => x.Movie)
                .FirstOrDefault(x => x.Id == id);
        }
        public async Task<ShowTime> GetShowTimeByIdAsync(int id)
        {
            return await _dbcontext.ShowTime
                .Include(x => x.Movie)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public bool IsSetting(int showTimeId,DateOnly dateShow, TimeSpan timeStart, TimeSpan timeEnd, int RoomId)
        {
            //bool check = true;
            //var listShowtime = _dbcontext.ShowTime.Where(x => x.RoomId == RoomId && x.DateShowTime == dateShow &&x.Id != showTimeId).ToList();
            //foreach (var item in listShowtime)
            //{
            //    if (timeStart <= item.StartTime && timeEnd >= item.StartTime)
            //    {
            //        check = false;
            //        break;
            //    }else if (timeEnd >= item.EndTime && timeStart <= item.EndTime)
            //    {
            //        check = false; break;
            //    }else if(timeStart>=item.StartTime && timeStart <= item.EndTime&&timeEnd>=item.StartTime&&timeEnd<=item.EndTime)
            //    {
            //        check = false; break;
            //    }

            //}
            //return check;
            return true;
        }

  

        public void UpdateisBooked(int showTimeId)
        {
            //var showTime=_dbcontext.ShowTime.Find(showTimeId);
            //showTime.isBooked = true;
            //_dbcontext.Update(showTime);
        }

        public void UpdateShowTime(ShowTime showTime)
        {
            _dbcontext.ShowTime.Update(showTime);
        }
    }
}