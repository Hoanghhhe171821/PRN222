using AssignmentPRN222.Interfaces;
using AssignmentPRN222.Models;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace AssignmentPRN222.Controllers
{
    public class OrderController : Controller
    {
        protected readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Payment(string selectedSeats, int showTimeId, int roomId)
        {
            if (string.IsNullOrEmpty(selectedSeats))
            {
                // Xử lý trường hợp không có ghế nào được chọn
                return BadRequest(); // Hoặc chuyển hướng về trang chọn ghế
            }

            var showTime = await _unitOfWork.ShowTimes.GetShowTimeByIdAsync(showTimeId);
            if (showTime == null)
            {
                return NotFound();
            }

            int totalPrice = 0;
            var selectedSeatIds = selectedSeats.Split(',').Select(int.Parse).ToList();

            foreach (var seatId in selectedSeatIds)
            {
                var seat = _unitOfWork.Seats.GetSeatById(seatId); // Lấy Seat trực tiếp
                if (seat == null)
                {
                    TempData["error"] = "Đã có lỗi xảy ra";
                    return RedirectToAction("Index", "Province"); // Xử lý lỗi nếu seat không tồn tại
                }

                if (seat.TypeSeat.Equals("VIP"))
                {
                    totalPrice += showTime.PriceB;
                }
                else
                {
                    totalPrice += showTime.PriceA;
                }
            }
            this.ViewBag.Price = totalPrice;
            this.ViewBag.selectedSeats = selectedSeats;
            this.ViewBag.showTimeId = showTimeId;
            this.ViewBag.roomId = roomId;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Payment(string selectedSeats, int showTimeId, int roomId, int price, string code)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (!string.IsNullOrEmpty(code))
                    {
                       await _unitOfWork.Discounts.updateStatus(code);

                    }

                    var selectedSeatIds = selectedSeats.Split(',').Select(int.Parse).ToList();
                    List<SeatsBooking> seatBookings = new List<SeatsBooking>();
                    foreach (var seatId in selectedSeatIds)
                    {
                        seatBookings.Add(new SeatsBooking
                        {
                            SeatId = seatId, // Gán seatId trực tiếp
                            ShowTimeId = showTimeId,
                            IsBooked = true
                        });
                    }
                    Order order = new Order
                    {
                        PriceFrist = price,
                        SeatsBookings = seatBookings,
                        UserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                    };
                    _unitOfWork.ShowTimes.UpdateisBooked(showTimeId);
                    _unitOfWork.Orders.Create(order);
                    _unitOfWork.Complete();
                    transaction.Complete();
                    TempData["Message"] = "Order is Success";
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    TempData["error"] = "Please login!";
                    return RedirectToAction("Index", "Home");
                }
            }
        }
    }
}
