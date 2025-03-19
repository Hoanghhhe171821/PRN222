using Microsoft.EntityFrameworkCore;
using Project_NH.Interfaces;
using Project_NH.Models;

namespace Project_NH.Repository
{
    public class OrderRepository : IOrder
    {
        protected readonly AppicationDbcontext _dbcontext;
        public OrderRepository(AppicationDbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public void Create(Order order)
        {
            _dbcontext.Orders.Add(order);
        }

        public List<Order> GetOrderByUserId(string userId)
        {
            List<Order> order = _dbcontext.Orders.Include(x=>x.SeatBookings).ThenInclude(st => st.ShowTime).ThenInclude(m=>m.Movie).Where(x=>x.UserId.Equals(userId)).ToList();
            if (order.Count > 0) { 
                return order;
            }
            else
            {
                return null;
            }

        }
    }
}
