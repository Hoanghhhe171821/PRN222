using AssignmentPRN222.Interfaces;
using AssignmentPRN222.Models;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentPRN222.Controllers
{
    public class MovieController : Controller
    {
        protected IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MovieController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index(string text = "", int page = 1, int pageSize = 5)
        {
            var allMovie = _unitOfWork.Movies.getMovieList(text);
            this.ViewBag.search = text;
            int totalItems = allMovie.Count;
            Pager pager = new Pager(totalItems, page, pageSize);
            var data = allMovie.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            this.ViewBag.Pager = pager;
            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Movie movie, IFormFile images)
        {
            if (!ModelState.IsValid)
            {

                return View();
            }
            if (images != null && images.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + images.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    images.CopyTo(fileStream);
                }

                movie.Images = "/images/" + uniqueFileName;
            }
            _unitOfWork.Movies.createMovice(movie);
            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }

        public IActionResult Detail(int movieId)
        {
            var movie = _unitOfWork.Movies.getMovieById(movieId);
            return View(movie);
        }
        public IActionResult Edit(int Id)
        {
            Movie movie = _unitOfWork.Movies.getMovieById(Id);
            return View(movie);
        }
        [HttpPost]
        public IActionResult Edit(Movie movie)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Movies.Update(movie);
                _unitOfWork.Complete();
                return RedirectToAction("Index");
            }
            return View();

        }
        public IActionResult Delete(int Id)
        {
            _unitOfWork.Movies.DeleteSoft(Id);
            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
        public IActionResult BuyTicket(int movieId, DateOnly dateBuy = default, int provinceId = 1)
        {
            if (dateBuy == default)
            {
                dateBuy = DateOnly.FromDateTime(DateTime.Now);
            }
            this.ViewBag.movieId = movieId;
            var listProvince = _unitOfWork.Provinces.getAll(); this.ViewBag.Provinces = listProvince;
            var listCinemabyProvinceID = _unitOfWork.Cinemas.getCinemaByIdProvince(provinceId);
            Dictionary<Cinema, List<ShowTime>> pairs = new Dictionary<Cinema, List<ShowTime>>();
            foreach (var cinema in listCinemabyProvinceID)
            {
                List<ShowTime> lis = _unitOfWork.ShowTimes.GetShowTimeByDateandCinemaandMovie(dateBuy, cinema.Id, movieId);
                pairs.Add(cinema, lis);
            }
            this.ViewBag.province = provinceId;
            this.ViewBag.Data = pairs;
            this.ViewBag.Date = dateBuy;
            return View();
        }
    }
}
