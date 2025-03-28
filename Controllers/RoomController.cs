using AssignmentPRN222.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentPRN222.Controllers
{
    public class RoomController : Controller
    {
        protected IUnitOfWork _unitOfWork;

        public RoomController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetCinemaByProvinceId(int provinceid)
        {
            var listCinema = _unitOfWork.Cinemas.getCinemaByIdProvince(provinceid);

            return Json(listCinema);
        }
    }
}
