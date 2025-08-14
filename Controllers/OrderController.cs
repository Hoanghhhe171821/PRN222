using AssignmentPRN222.Interfaces;
using AssignmentPRN222.Models;
using AssignmentPRN222.Models.Vnpay;
using AssignmentPRN222.Services.Vnpay;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AssignmentPRN222.Controllers
{
    public class OrderController : Controller
    {
        protected readonly IUnitOfWork _unitOfWork;
        private readonly IVnPayService _vnPayService;
        public OrderController(IUnitOfWork unitOfWork, IVnPayService vnPayService)
        {
            _unitOfWork = unitOfWork;
            _vnPayService = vnPayService;
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
        public async Task<IActionResult> Payment(string selectedSeats, int showTimeId, int price, string code)
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
                        UserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
                        CreatedAt = DateTime.Now,
                        PaymentMethod="Thanh toán tại quầy",
                        Status = "Pending"                        
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
        [HttpPost]
        public IActionResult CreatePaymentUrlVnpay(PaymentInformationModel model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

            return Redirect(url);
        }
        [HttpGet]
        public async Task<IActionResult> PaymentCallbackVnpay()
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var response = _vnPayService.PaymentExecute(Request.Query);

                    if (!response.Success || response.VnPayResponseCode != "00")
                    {
                        TempData["error"] = "Thanh toán thất bại!";
                        return RedirectToAction("Index", "Home");
                    }
                    // Format: "{SelectedSeats}_{ShowTimeId}_{PaymentMethod}_{Price}"
                    var orderInfoParts = response.OrderDescription.Split('_');
                    if (orderInfoParts.Length != 4)
                    {
                        TempData["error"] = "Dữ liệu thanh toán không hợp lệ!";
                        return RedirectToAction("Index", "Home");
                    }
                    var selectedSeatsStr = orderInfoParts[0].Replace("-", ",");
                    var showTimeId = int.Parse(orderInfoParts[1]);
                    var paymentMethod = orderInfoParts[2];
                    var price = int.Parse(orderInfoParts[3]);

                    var selectedSeatIds = selectedSeatsStr.Split(',').Select(int.Parse).ToList();
                    List<SeatsBooking> seatBookings = new List<SeatsBooking>();
                    foreach (var seatId in selectedSeatIds)
                    {
                        seatBookings.Add(new SeatsBooking
                        {
                            SeatId = seatId,
                            ShowTimeId = showTimeId,
                            IsBooked = true
                        });
                    }
                    var order = new Order
                    {
                        PriceFrist = price,
                        SeatsBookings = seatBookings,
                        UserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
                        CreatedAt = DateTime.Now,
                        PaymentMethod = paymentMethod,
                        Status = "Completed"
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
                    // Log lỗi
                    return Json(new
                    {
                        Success = false,
                        Message = "Error processing VNPay callback",
                        Error = ex.Message
                    });
                }
            }
        }

    }
}
