using Microsoft.EntityFrameworkCore;
using AssignmentPRN222.Interfaces;
using AssignmentPRN222.Models;

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

        public List<Order> GetOrderByUserId(string userId)
        {
            List<Order> order = _dbcontext.Orders.Include(x=>x.SeatsBookings).ThenInclude(st => st.ShowTime).ThenInclude(m=>m.Movie).Where(x=>x.UserId.Equals(userId)).ToList();
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
