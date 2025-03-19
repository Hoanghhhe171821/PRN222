using Microsoft.EntityFrameworkCore;
using Project_NH.Interfaces;
using Project_NH.Models;

namespace Project_NH.Repository
{
    public class CinemaRepository : ICinema
    {
        protected readonly AppicationDbcontext _dbcontext;
        public CinemaRepository(AppicationDbcontext dbcontext)
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
            cinema.isDelete = true;
            _dbcontext.Update(cinema);
        }

        public List<Cinema> getAllCinema(int provinceId,string text)
        {
            return _dbcontext.Cinemas.Where(x=>!x.isDelete && (x.ProvinceId == provinceId || provinceId == 0) && x.Name.Contains(text)).Include(x=>x.Province).ToList();
        }

        public Cinema getCinema(int id)
        {
           return _dbcontext.Cinemas.Find(id);
        }

        public List<Cinema> getCinemaByIdProvince(int provinceId)
        {
            return _dbcontext.Cinemas.Where(x => x.ProvinceId == provinceId&& !x.isDelete).ToList();
        }

        public void Update(Cinema cinema)
        {
            _dbcontext.Update(cinema);
        }
    }
}
