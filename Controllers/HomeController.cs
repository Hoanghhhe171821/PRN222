using Microsoft.AspNetCore.Mvc;

namespace AssignmentPRN222.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Message = TempData["Message"];
            return View();
        }
    }
}
