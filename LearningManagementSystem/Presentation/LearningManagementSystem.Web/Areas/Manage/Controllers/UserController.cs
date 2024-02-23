using LearningManagementSystem.Application.Abstraction.Services;
using LearningManagementSystem.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Web.Areas.Manage.Controllers
{
	[Area("manage")]
	public class UserController : Controller
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}
		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterVm vm)
		{
			var result = await _userService.RegisterAsync(vm, ModelState);
			if (result) return RedirectToAction("index", vm.Role.ToString());
			return View(vm);
		}
		public async Task<IActionResult> CreateRole()
		{
			await _userService.CreateRoleAsync();
			return RedirectToAction("index", "dashboard");
		}
		public async Task<IActionResult> ResetPasswordDefaultTeacher(int id)
		{
			await _userService.ResetPasswordDefaultTeacher(id);
			return Ok();
		}
		public async Task<IActionResult> ResetPasswordDefaultStudent(int id)
		{
			await _userService.ResetPasswordDefaultStudent(id);
			return Ok();
		}
	}
}
