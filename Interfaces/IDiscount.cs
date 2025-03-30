using AssignmentPRN222.Models;

namespace AssignmentPRN222.Interfaces
{
    public interface IDiscount
    {
        List<Discount> GetDiscounts();
        void Create(Discount discount);
        int getPriceDiscount(string guid);
        Task updateStatus(string code);
    }
}
