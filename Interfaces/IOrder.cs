using AssignmentPRN222.Models;

namespace AssignmentPRN222.Interfaces
{
    public interface IOrder
    {
        void Create(Order order);
        List<Order> GetOrderByUserId(string userId);
    }
}
