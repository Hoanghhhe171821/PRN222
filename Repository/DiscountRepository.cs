using AssignmentPRN222.Interfaces;
using AssignmentPRN222.Models;
using Microsoft.EntityFrameworkCore;

namespace AssignmentPRN222.Repository
{
    public class DiscountRepository : IDiscount
    {
        protected readonly ProjectPrn222Context _dbcontext;
        public DiscountRepository(ProjectPrn222Context dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public void Create(Discount discount)
        {
            _dbcontext.Add(discount);
        }

        public List<Discount> GetDiscounts()
        {
            return _dbcontext.Discounts.OrderBy(x => x.IsDiscounted)
                                        .ThenByDescending(x => x.Id)
                                        .ToList();
        }

        public int getPriceDiscount(string code)
        {
            var priceDiscount = _dbcontext.Discounts.Where(x => x.Name.Equals(code)).FirstOrDefault();
            if (priceDiscount != null && !priceDiscount.IsDiscounted)
            {
                return priceDiscount.DiscountPrice;
            }
            return 0;
        }

        public async Task updateStatus(string code)
        {
            var discount = _dbcontext.Discounts.FirstOrDefault(x => x.Name.Equals(code));
            if (discount != null && discount.Quantity > 0)
            {
                discount.Quantity--;
                if (discount.Quantity <= 0)
                {
                    discount.IsDiscounted = true;
                }
            }
            _dbcontext.SaveChanges();
        }
        public async Task<Discount?> GetById(int id)
        {
            return await _dbcontext.Discounts.FindAsync(id);
        }
        public async Task<bool> Update(Discount discount)
        {
            var existingDiscount = await _dbcontext.Discounts.FindAsync(discount.Id);
            if (existingDiscount == null)
            {
                return false;
            }

            existingDiscount.Name = discount.Name;
            existingDiscount.DiscountPrice = discount.DiscountPrice;
            existingDiscount.Quantity = discount.Quantity;
            existingDiscount.IsDiscounted = discount.Quantity > 0 ? false : true;

            await _dbcontext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> Delete(int id)
        {
            var discount = await _dbcontext.Discounts.FindAsync(id);
            if (discount == null)
            {
                return false;
            }

            _dbcontext.Discounts.Remove(discount);
            await _dbcontext.SaveChangesAsync();
            return true;
        }
        public async Task<Discount?> GetByName(string name)
        {
            return await _dbcontext.Discounts.FirstOrDefaultAsync(d => d.Name != null && d.Name.Equals(name));
        }
    }
}
