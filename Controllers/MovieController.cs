using AssignmentPRN222.Interfaces;
using AssignmentPRN222.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "admin")]    
        public IActionResult Index(string text = "", int page = 1, int pageSize = 5)
        {
            var allMovie = _unitOfWork.Movies.getMovieListByAdmin(text);
            this.ViewBag.search = text;
            int totalItems = allMovie.Count;
            Pager pager = new Pager(totalItems, page, pageSize);
            var data = allMovie.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            this.ViewBag.Pager = pager;
            return View(data);
        }
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Create(Movie movie, IFormFile images)
        {
            if (!ModelState.IsValid)
            {

                return View();
            }
            if (images != null && images.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + images.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    images.CopyTo(fileStream);
                }

                movie.Images = "/img/" + uniqueFileName;
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
        [Authorize(Roles = "admin")]
        public IActionResult Edit(int Id)
        {
            Movie movie = _unitOfWork.Movies.getMovieById(Id);
            return View(movie);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Edit(Movie movie,IFormFile imagesFile)
        {
            ModelState.Remove("Images");
            if (ModelState.IsValid)
            {
                if (imagesFile != null && imagesFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + imagesFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        imagesFile.CopyTo(stream);
                    }
                    movie.Images = "/img/" + uniqueFileName;
                }
                else
                {
                    var existingMovie = _unitOfWork.Movies.getMovieById(movie.Id);
                    movie.Images = existingMovie.Images;
                }

                _unitOfWork.Movies.Update(movie);
                _unitOfWork.Complete();
                return RedirectToAction("Index");
            }
            return View(movie);

        }
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int Id)
        {
            _unitOfWork.Movies.DeleteSoft(Id);
            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "admin")]
        public IActionResult UnDelete(int Id)
        {
            _unitOfWork.Movies.UnDeleteSoft(Id);
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
