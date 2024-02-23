using LearningManagementSystem.Application.Abstraction.Services;
using LearningManagementSystem.Application.Utilities.Exceptions;
using LearningManagementSystem.Application.ViewModels;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Persistance.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Xml.Linq;

namespace LearningManagementSystem.Web.Controllers
{
	public class TeacherController : Controller
	{
		private readonly ITeacherService _service;
		private readonly IGroupService _groupService;
		private readonly IStudentService _studentService;


		public TeacherController(ITeacherService service, IGroupService groupService, IStudentService studentService)
		{
			_service = service;
			_groupService = groupService;
			_studentService = studentService;
			
		}
		public async Task<IActionResult> Index()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			return View(await _service.GetGroupsAsync(userId));
		}
		public async Task<IActionResult> Group(int id)
		{
			GroupItemVm vm = await _groupService.GetGroupItems(id);
			if (vm == null) return NotFound();			
			return View(vm);
		}		
		public async Task<IActionResult> Profile()
		{
			var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			UpdateTeacherVm vm = new UpdateTeacherVm();
			vm = await _service.GetTeacherInfo(userid, vm);
			return View(vm);
		}
		public async Task<IActionResult> Edit()
		{
			var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			UpdateTeacherVm vm = new UpdateTeacherVm();
			vm = await _service.TeacherForEditAsync(userid, vm);
			return View(vm);
		}
		[HttpPost]
		public async Task<IActionResult> Edit(UpdateTeacherVm vm)
		{
			var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (await _service.TeacherEditAsync(userid, vm, ModelState))
				return RedirectToAction(nameof(Profile));
			return View(await _service.TeacherForEditAsync(userid, vm));
		}


	}
}
