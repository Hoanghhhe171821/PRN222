using Microsoft.EntityFrameworkCore;
using Project_NH.Interfaces;
using Project_NH.Models;

namespace Project_NH.Repository
{
    public class ProvinceRepository : IProvince
    {
        protected readonly AppicationDbcontext _dbcontext;
        public ProvinceRepository(AppicationDbcontext dbcontext) {
            _dbcontext = dbcontext;
        }
        public List<ProVince> getAll()
        {
            return _dbcontext.ProVinces.Include(p =>p.Cinemas).ToList();
        }

        
    }
}
