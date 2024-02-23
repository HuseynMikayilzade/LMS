using LearningManagementSystem.Application.Abstraction.Services;
using LearningManagementSystem.Application.Utilities.Extentions;
using LearningManagementSystem.Application.ViewModels;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Security.Claims;
using static LearningManagementSystem.Web.Areas.Manage.Controllers.AssignmentController;

namespace LearningManagementSystem.Web.Controllers
{

    public class AccountController : BaseController
    {
        private readonly IUserService _userService;

        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;

        public AccountController(IUserService userService, UserManager<AppUser> userManager, IEmailService emailService)
        {
            _userService = userService;
            _userManager = userManager;
            _emailService = emailService;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVm vm)
        {
            var result = await _userService.LoginAsync(vm, ModelState);
            if (!result) return View(vm);
            var islogin = await _userService.IsLoginAsync(vm.Email);
            if (!islogin)
            {
                return RedirectToAction(nameof(ResetPasswordForFirstLogin));
            }
            else
            {
                return RedirectToIndexBasedOnRole();
            }
        }
        public async Task<IActionResult> LogOut()
        {
            await _userService.LogOutAsync();
            return RedirectToAction("Index", "Home");
        }

        //============== RESET PASSWORD FOR FIRST LOGIN ==============//
        public IActionResult ResetPasswordForFirstLogin()
        {
            return View("ResetPassword");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPasswordForFirstLogin(ResetPasswordVm vm)
        {
            var result = await _userService.FirstLoginForResetPasswordAsync(vm, User.Identity.Name, ModelState);
            if (!result) return View("ResetPassword", vm);
            return RedirectToIndexBasedOnRole();
        }
        //============== Reset PASSWORD ==============//


        public async Task<IActionResult> ResetPassword(string userid, string token)
        {
            if (string.IsNullOrWhiteSpace(userid) || string.IsNullOrWhiteSpace(token)) return BadRequest();
            AppUser user = await _userManager.FindByIdAsync(userid);
            if (user == null) return NotFound();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVm vm, string userid, string token)
        {
            if (string.IsNullOrWhiteSpace(userid) || string.IsNullOrWhiteSpace(token)) return BadRequest();
            if (!ModelState.IsValid) return View(vm);
            AppUser user = await _userManager.FindByIdAsync(userid);
            if (user == null) return NotFound();
            var result = await _userManager.ResetPasswordAsync(user, token, vm.ConfirmPassword);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
                return View(vm);
            }
            return RedirectToAction(nameof(Login));
        }

        //============== Forgot PASSWORD ==============//

        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVm vm)
        {
            if (!ModelState.IsValid) return View(vm);
            AppUser user = await _userManager.FindByEmailAsync(vm.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User Not Found");
                return View(vm);
            }
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string link = Url.Action("ResetPassword", "Account", new { userId = user.Id, token = token }, HttpContext.Request.Scheme);
            string emailbody = EmailBodyCreator.EmailBody(link);
            await _emailService.SendEmailAsync(user.Email, "Reset Password", emailbody, true);
            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        public async Task<IActionResult> ResetProfilePassword([FromForm] ResetPasswordVm data)
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (await _userService.ResetProfilePassword(userid, data, ModelState))
            {
                return Ok(new returnData { statusCode = 200, message = "Success" });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                var errorMessage = string.Empty;
                foreach (var item in errors)
                {
                    errorMessage += item.ErrorMessage + "\n";
                }
                return Ok(new returnData { statusCode = 400, message = errorMessage });
            }   
        }
    }
}
