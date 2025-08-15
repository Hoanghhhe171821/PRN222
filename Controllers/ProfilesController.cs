using AssignmentPRN222.Interfaces;
using AssignmentPRN222.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentPRN222.Controllers
{
    public class ProfilesController : Controller
    {
        protected readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<UserProfile> _userManager;
        private readonly SignInManager<UserProfile> _signInManager;
        public ProfilesController(IUnitOfWork unitOfWork,
                          UserManager<UserProfile> userManager,
                          SignInManager<UserProfile> signInManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            var userProfile = _unitOfWork.User.GetUserByUserId(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            if (userProfile == null)
            {
                TempData["Message"] = "Pls Login!";
                return RedirectToAction("Index", "Home");
            }
            return View(userProfile);
        }
        [HttpPost]
        public async Task<IActionResult> Index(UserProfile model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = await _userManager.FindByIdAsync(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            if (user == null)
            {
                TempData["Message"] = "User not found!";
                return RedirectToAction("Index", "Home");
            }

            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["Message"] = "Profile updated successfully!";
                return RedirectToAction("Index","Home"); 
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
        }

        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Message"] = "User not found!";
                return RedirectToAction("Index", "Home");
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user); // refresh session
                TempData["Message"] = "Password changed successfully!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }

    }
}
