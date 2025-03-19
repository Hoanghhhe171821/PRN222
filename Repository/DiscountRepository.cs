using Project_NH.Interfaces;
using Project_NH.Models;

namespace Project_NH.Repository
{
    public class DiscountRepository : IDiscount
    {
        protected readonly AppicationDbcontext _dbcontext;
        public DiscountRepository(AppicationDbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public void Create(Discount discount)
        {
            _dbcontext.Add(discount);
        }

        public List<Discount> GetDiscounts()
        {
            return _dbcontext.Discounts.Where(x=>!x.IsDiscounted).ToList();
        }

        public int getPriceDiscount(string code)
        {
            var priceDiscount = _dbcontext.Discounts.Where(x => x.Name.Equals(code)).FirstOrDefault();
            if (priceDiscount != null && !priceDiscount.IsDiscounted ) {
                return priceDiscount.DiscountPrice;
            }
            return 0;
        }

        public bool updateStatus(string code)
        {
            var discount =_dbcontext.Discounts.FirstOrDefault(x=>x.Name.Equals(code));
            if(discount!=null && !discount.IsDiscounted)
            {
                discount.IsDiscounted = true;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
