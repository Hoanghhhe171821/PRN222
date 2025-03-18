//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using AssignmentPRN222.Interfaces;
//using AssignmentPRN222.Models;
//using AssignmentPRN222.Repository;
//using System.Security.Claims;

//namespace AssignmentPRN222.Areas.Identity.Pages.Account.Manage
//{
//    public class HistoryModel : PageModel
//    {
//        protected readonly IUnitOfWork _unitOfWork;
//        public List<Order> orders { get; set; }

//        public HistoryModel(IUnitOfWork unitOfWork) {
//            _unitOfWork = unitOfWork;       
//        }
//        public void OnGet()
//        {
//            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//            orders = _unitOfWork.Orders.GetOrderByUserId(userId);
//        }
//    }
//}
