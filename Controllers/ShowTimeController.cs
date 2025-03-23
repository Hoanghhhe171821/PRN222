using AssignmentPRN222.Interfaces;
using AssignmentPRN222.Models;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentPRN222.Controllers
{
    public class ShowTimeController : Controller
    {
        protected readonly IUnitOfWork _unitOfWork;
        public ShowTimeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(int movieId = 0, int cinemaid = 0, int roomId = 0, int pageSize = 5, int page = 1)
        {
            this.ViewBag.MovieId = movieId;
            this.ViewBag.CinemaId = cinemaid;
            this.ViewBag.RoomId = roomId;
            var cinemalist = _unitOfWork.Cinemas.getAllCinema(0, ""); this.ViewBag.cinemas = cinemalist;
            var listmovie = _unitOfWork.Movies.getMovieList(""); this.ViewBag.movies = listmovie;
            var allShowTime = _unitOfWork.ShowTimes.GetAll(movieId, cinemaid, roomId);
            int totalItems = allShowTime.Count;
            var pager = new Pager(totalItems, page, pageSize);
            var listShowTime = allShowTime.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            this.ViewBag.Pager = pager;
            return View(listShowTime);
        }
        public IActionResult Create()
        {
            var listMovie = _unitOfWork.Movies.getMovieList("");
            this.ViewBag.Movies = listMovie;
            var listProvince = _unitOfWork.Provinces.getAll();
            this.ViewBag.Provinces = listProvince;
            return View();
        }
        [HttpPost]
        public IActionResult Create(ShowTime showTime)
        {
            var movie = _unitOfWork.Movies.getMovieById(showTime.MovieId);
            bool ischeck = _unitOfWork.ShowTimes.IsSetting(showTime.Id, showTime.DateShowTime, showTime.StartTime, showTime.EndTime, showTime.RoomId);
            if (!ischeck)
            {
                ModelState.AddModelError("StartTime", "This time is busy");
            }

            if (ModelState.IsValid)
            {

                var listSeat = _unitOfWork.Seats.GetSeatList(showTime.RoomId);
                showTime.EndTime = showTime.StartTime + movie.TimeMovie.ToTimeSpan();
                _unitOfWork.ShowTimes.Create(showTime);
                _unitOfWork.Complete();
                return RedirectToAction("Index");
            }
            var listMovie = _unitOfWork.Movies.getMovieList("");
            this.ViewBag.Movies = listMovie;
            var listProvince = _unitOfWork.Provinces.getAll();
            this.ViewBag.Provinces = listProvince;
            return View();
        }
        public IActionResult Edit(int Id)
        {
            ShowTime showTime = _unitOfWork.ShowTimes.GetShowTimeById(Id);
            return View(showTime);
        }
        [HttpPost]
        public IActionResult Edit(ShowTime showTime)
        {
            var movie = _unitOfWork.Movies.getMovieById(showTime.MovieId);
            showTime.EndTime = showTime.StartTime + movie.TimeMovie.ToTimeSpan();
            bool ischeck = _unitOfWork.ShowTimes.IsSetting(showTime.Id, showTime.DateShowTime, showTime.StartTime, showTime.EndTime, showTime.RoomId);
            if (!ischeck)
            {
                ModelState.AddModelError("StartTime", "This time is busy");
            }
            _unitOfWork.ShowTimes.UpdateShowTime(showTime);
            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int Id)
        {
            _unitOfWork.ShowTimes.DeleteShowTime(Id);
            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }



        [HttpGet]
        public IActionResult GetRoomByCinemaId(int cinemaid)
        {
            var listRoom = _unitOfWork.Rooms.GetRoomByCinemaId(cinemaid);

            return Json(listRoom);
        }


    }
}
