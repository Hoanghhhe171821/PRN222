using AssignmentPRN222.Models;
using System.Collections.Generic;

namespace AssignmentPRN222.Interfaces
{
    public interface IShowTime
    {
         void Create(ShowTime showTime);        
         List<ShowTime> GetAll(int movieId, int cinemaId, int roomId);
         bool IsSetting(int showTimeId, DateOnly dateShow,TimeSpan timeStart,TimeSpan timeEnd,int RoomId);
         List<ShowTime> GetShowTimeByDateandCinemaandMovie(DateOnly date, int cinemaid,int movieId);
         ShowTime GetShowTimeById(int id);
         void UpdateisBooked(int showTimeId);
         void UpdateShowTime(ShowTime showTime);
         void DeleteShowTime(int showTimeId);
        


    }

    
}
