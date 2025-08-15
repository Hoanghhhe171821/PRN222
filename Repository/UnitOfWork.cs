using Microsoft.EntityFrameworkCore;
using AssignmentPRN222.Interfaces;
using AssignmentPRN222.Models;

namespace AssignmentPRN222.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProjectPrn222Context _context;
        public IProvince Provinces { get; private set; }
        public ICinema Cinemas { get; private set; }
        public IMovie Movies { get; private set; }
        public ISeat Seats { get; private set; }
        public IShowTime ShowTimes { get; private set; }    
        public IRoom Rooms { get; private set; }
        public ISeatBooking SeatBookings { get; private set; }
        public IDiscount Discounts { get; private set; }
        public IOrder Orders { get; private set; }
        public IUser User { get; private set; }
        public UnitOfWork(ProjectPrn222Context context, IProvince province,ICinema cinema,
            IMovie movie,ISeat seat,IShowTime showTime,IRoom room,ISeatBooking seatBooking,IDiscount discount,
            IOrder order,IUser user)
        {
            _context = context;
            Provinces = province;
            Cinemas = cinema;
            Movies = movie;
            Seats = seat;
            ShowTimes = showTime;
            Rooms = room;
            SeatBookings = seatBooking;
            Discounts = discount;
            Orders = order;
            User = user;
        }


        public int Complete()
        {
            return _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
