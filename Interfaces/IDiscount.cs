using AssignmentPRN222.Models;

namespace AssignmentPRN222.Interfaces
{
    public interface IDiscount
    {
        List<Discount> GetDiscounts();
        void Create(Discount discount);
        int getPriceDiscount(string guid);
        Task updateStatus(string code);
        Task<Discount?> GetById(int id);
        Task<bool> Update(Discount discount);
        Task<bool> Delete(int id);
        Task<Discount?> GetByName(string name);
    }
}
