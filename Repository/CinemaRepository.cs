using Microsoft.EntityFrameworkCore;
using AssignmentPRN222.Interfaces;
using AssignmentPRN222.Models;

namespace AssignmentPRN222.Repository
{
    public class CinemaRepository : ICinema
    {
        protected readonly ProjectPrn222Context _dbcontext;
        public CinemaRepository(ProjectPrn222Context dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public bool Create(Cinema cinema)
        {
            if (cinema == null)
            {
                return false;
            }
            try
            {
                _dbcontext.Add(cinema);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public void DeleteSoft(int id)
        {
            var cinema=_dbcontext.Cinemas.Find(id);
            cinema.IsDelete = true;
            _dbcontext.Update(cinema);
        }

        public List<Cinema> getAllCinema(int provinceId,string text)
        {
            return _dbcontext.Cinemas.Where(x=>!x.IsDelete && (x.ProvinceId == provinceId || provinceId == 0) && x.Name.Contains(text)).Include(x=>x.Province).ToList();
        }

        public Cinema getCinema(int id)
        {
           return _dbcontext.Cinemas.Find(id);
        }

        public List<Cinema> getCinemaByIdProvince(int provinceId)
        {
            return _dbcontext.Cinemas.Where(x => x.ProvinceId == provinceId&& !x.IsDelete).ToList();
        }

        public void Update(Cinema cinema)
        {
            _dbcontext.Update(cinema);
        }
    }
}
