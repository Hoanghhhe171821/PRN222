using AssignmentPRN222.Interfaces;
using AssignmentPRN222.Models;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentPRN222.Controllers
{
    public class DiscountController : Controller
    {
        protected IUnitOfWork _unitOfWork;

        public DiscountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(int pageSize = 5, int page = 1)
        {
            var allitems = _unitOfWork.Discounts.GetDiscounts();
            int totalitems = allitems.Count();
            var pager = new Pager(totalitems, page, pageSize);
            var listDiscount = allitems.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            this.ViewBag.Pager = pager;
            return View(listDiscount);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Discount discount, string Quantity, string Name)
        {
            if (string.IsNullOrEmpty(Quantity))
            {
                ModelState.AddModelError("Quantity", "Quantity is required.");
                return View();
            }

            if (!int.TryParse(Quantity, out int quantityValue))
            {
                ModelState.AddModelError("Quantity", "Quantity must be a valid number.");
                return View();
            }

            if (quantityValue < 0)
            {
                ModelState.AddModelError("Quantity", "Quantity must be greater than or equal to 0.");
            }

            if (ModelState.IsValid)
            {
                Discount d = new Discount();
                d.Name = Name;
                d.DiscountPrice = discount.DiscountPrice;
                d.Quantity = quantityValue;
                _unitOfWork.Discounts.Create(d);
                _unitOfWork.Complete();
                return RedirectToAction("Index", "Discount");
            }

            return View();
        }
        [HttpGet]
        public IActionResult getDiscount(string code)
        {
            var discount = _unitOfWork.Discounts.getPriceDiscount(code);
            if (discount != null && discount != 0)
            {
                return Json(new { success = true, price = discount });
            }
            return Json(new { success = false, message = "Invalid code" });
        }
    }
}

