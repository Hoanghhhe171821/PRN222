using AssignmentPRN222.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentPRN222.Controllers
{
    public class MovieListController : Controller
    {
        protected IUnitOfWork _unitOfWork;

        public MovieListController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public IActionResult Index()
        {
            var listMovie = _unitOfWork.Movies.getMovieList("");
            return View(listMovie);
        }
    }
}
