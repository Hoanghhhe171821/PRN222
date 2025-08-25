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

        public List<ShowTime> GetAll(int movieId, int cinemaId, int roomId)
        {
            var now = DateTime.Now;

            var query = _dbcontext.ShowTime
                .Where(x => (x.MovieId == movieId || movieId == 0) &&
                            (x.CinemaId == cinemaId || cinemaId == 0) &&
                            (x.RoomId == roomId || roomId == 0))
                .Include(x => x.Movie);

            // Bước 2: xử lý ngoài memory
            return query
                .AsEnumerable()
                .OrderBy(x =>
                {
                    var showDateTime = x.DateShowTime.ToDateTime(TimeOnly.MinValue).Add(x.StartTime);
                    return showDateTime < now ? 1 : 0; // chưa qua = 0, đã qua = 1
                })
                .ThenBy(x => x.DateShowTime)
                .ThenBy(x => x.StartTime)
                .ToList();
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

        public bool IsSetting(int showTimeId, DateOnly dateShow, TimeSpan timeStart, TimeSpan timeEnd, int roomId)
        {
            var listShowtime = _dbcontext.ShowTime
                .Where(x => x.RoomId == roomId
                         && x.DateShowTime == dateShow
                         && x.Id != showTimeId)
                .ToList();

            foreach (var item in listShowtime)
            {
                // nếu KHÔNG kết thúc trước hoặc sau hẳn -> nghĩa là giao nhau
                if (!(timeEnd <= item.StartTime || timeStart >= item.EndTime))
                {
                    return false; // trùng lịch
                }
            }

            return true; // không trùng
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