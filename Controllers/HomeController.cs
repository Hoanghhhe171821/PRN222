using Microsoft.AspNetCore.Mvc;

namespace AssignmentPRN222.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
