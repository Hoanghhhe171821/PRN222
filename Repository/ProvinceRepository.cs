using Microsoft.EntityFrameworkCore;
using AssignmentPRN222.Interfaces;
using AssignmentPRN222.Models;

namespace AssignmentPRN222.Repository
{
    public class ProvinceRepository : IProvince
    {
        protected readonly ProjectPrn222Context _dbcontext;
        public ProvinceRepository(ProjectPrn222Context dbcontext) {
            _dbcontext = dbcontext;
        }
        public List<ProVince> getAll()
        {
            return _dbcontext.ProVinces.Include(p =>p.Cinemas).ToList();
        }

        
    }
}
