using Microsoft.EntityFrameworkCore;
using AssignmentPRN222.Interfaces;
using AssignmentPRN222.Models;
using AssignmentPRN222.Dtos;

namespace AssignmentPRN222.Repository
{
    public class OrderRepository : IOrder
    {
        protected readonly ProjectPrn222Context _dbcontext;
        public OrderRepository(ProjectPrn222Context dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public void Create(Order order)
        {
            _dbcontext.Orders.Add(order);
        }

        public async Task<List<Order>> GetOrderByUserId(string userId)
        {
            return await _dbcontext.Orders
                .Include(o => o.SeatsBookings).ThenInclude(st => st.Seat)
                .Include(o => o.SeatsBookings).ThenInclude(st => st.ShowTime).ThenInclude(m => m.Movie)
                .Where(x => x.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }
        public async Task<OrderDetailsVM> GetOrderById(int orderId,string userId)
        {
            var order = await _dbcontext.Orders
                            .Where(x => x.Id == orderId && x.UserId == userId)
                            .Select(o => new OrderDetailsVM
                            {
                                Id = o.Id,
                                CreatedAt = o.CreatedAt,
                                PriceFrist = o.PriceFrist,
                                PaymentMethod = o.PaymentMethod,
                                Status = o.Status,
                                Seats = o.SeatsBookings.Select(sb => new SeatBookingVM
                                {
                                    SeatRow = sb.Seat.RowNumber,
                                    SeatClo = sb.Seat.CloNumber,
                                    SeatType = sb.Seat.TypeSeat,
                                    RoomName = sb.Seat.Room.RoomName,
                                    CinemaName = sb.Seat.Room.Cinema.Name,
                                    ProvinceName = sb.Seat.Room.Cinema.Province.Name,
                                    MovieName = sb.ShowTime.Movie.Name,
                                    DateShowTime = sb.ShowTime.DateShowTime,
                                    StartTime = sb.ShowTime.StartTime,
                                    TicketNo = sb.ShowTime.Id
                                }).ToList()
                            }).FirstOrDefaultAsync();

            return order;
        }
    }
}
