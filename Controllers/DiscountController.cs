using AssignmentPRN222.Interfaces;
using AssignmentPRN222.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "admin")]
        public IActionResult Index(int pageSize = 5, int page = 1)
        {
            var allitems = _unitOfWork.Discounts.GetDiscounts();
            int totalitems = allitems.Count();
            var pager = new Pager(totalitems, page, pageSize);
            var listDiscount = allitems.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            this.ViewBag.Pager = pager;
            return View(listDiscount);
        }
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create(Discount discount, string Quantity, string Name)
        {
            if (string.IsNullOrEmpty(Name))
            {
                ModelState.AddModelError("Name", "Discount Code is required.");
                return View();
            }

            if ( await _unitOfWork.Discounts.GetByName(Name) != null)
            {
                ModelState.AddModelError("Name", "Discount Code đã tồn tại.");
                return View();
            }

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

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var discount = await _unitOfWork.Discounts.GetById(id);
            if (discount == null)
            {
                return NotFound();
            }
            return View(discount);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(Discount discount)
        {
            if (!ModelState.IsValid)
            {
                return View(discount);
            }

            var success = await _unitOfWork.Discounts.Update(discount);
            if (!success)
            {
                TempData["Error"] = "Update failed. Discount not found.";
                return RedirectToAction("Index");
            }

            TempData["Message"] = "Discount updated successfully!";
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            bool isDeleted = await _unitOfWork.Discounts.Delete(id);
            if (!isDeleted)
            {
                TempData["error"] = "Không tìm thấy mã giảm giá!";
            }
            else
            {
                TempData["success"] = "Xóa mã giảm giá thành công!";
            }
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> UnDelete(int id)
        {
            bool isDeleted = await _unitOfWork.Discounts.UnDelete(id);
            if (!isDeleted)
            {
                TempData["error"] = "Không tìm thấy mã giảm giá!";
            }
            else
            {
                TempData["success"] = "Xóa mã giảm giá thành công!";
            }
            return RedirectToAction("Index");
        }
    }
}

