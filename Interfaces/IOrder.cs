using AssignmentPRN222.Dtos;
using AssignmentPRN222.Models;

namespace AssignmentPRN222.Interfaces
{
    public interface IOrder
    {
        void Create(Order order);
        Task<List<Order>> GetOrderByUserId(string userId);
        Task<OrderDetailsVM> GetOrderById(int orderId, string userId);
    }
}
