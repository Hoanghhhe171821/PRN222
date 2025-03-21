using AssignmentPRN222.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AssignmentPRN222.Models;
namespace AssignmentPRN222.Controllers
{
    public class SeatBookingController : Controller
    {
        protected readonly IUnitOfWork _unitOfWork;

        public SeatBookingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Detail(int showTimeId)
        {
            var showTime = await _unitOfWork.ShowTimes.GetShowTimeByIdAsync(showTimeId); // Lấy thông tin suất chiếu, bao gồm RoomId
            if (showTime == null)
            {
                return NotFound();
            }

            var seats = await _unitOfWork.Seats.GetSeatListAsync(showTime.RoomId); // Lấy tất cả ghế trong phòng
            var bookedSeats = await _unitOfWork.SeatBookings.GetList(showTimeId); // Lấy danh sách ghế ĐÃ ĐẶT

            // Truyền dữ liệu ghế và ghế đã đặt tới View
            var viewModel = new SeatBookingViewModel
            {
                ShowTime = showTime,
                Seats = seats,
                BookedSeatIds = bookedSeats.Select(b => b.SeatId).ToList()
            };

            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Detail(string selectedSeats, int showTimeId, int roomId)
        {
            return RedirectToAction("Payment", "Order", new { selectedSeats = selectedSeats, showTimeId = showTimeId, roomId = roomId });
        }
    }
}
