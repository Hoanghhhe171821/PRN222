using AssignmentPRN222.Interfaces;
using AssignmentPRN222.Models;

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
            return _dbcontext.Discounts.Where(x => !x.IsDiscounted).ToList();
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
    }
}
