using LearningManagementSystem.Application.Abstraction.Services;
using LearningManagementSystem.Application.Utilities.Exceptions;
using LearningManagementSystem.Application.ViewModels;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Persistance.DAL;
using LearningManagementSystem.Web.Areas.Manage.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Web.Controllers
{
    public class AttendanceController : Controller
    {
		
		private readonly IAttendanceService _service;

		public AttendanceController(IAttendanceService service)
        {
		
			_service = service;
		}
        public async Task<IActionResult> Index(int groupid,DateTime date)
        {	
            return View(await _service.GetAttendanceAsync(groupid,date));
        }
        public async Task<IActionResult> Create(int id)
        {
			CreateAttendanceVm vm = await _service.GetCreateAsync(id);
			if (vm == null) return NotFound();
            return View(vm);
        }
		[HttpPost]
		public async Task<IActionResult> Create(int id, CreateAttendanceVm vm)
		{
			if(await _service.CreateAsync(id,vm,ModelState))
				return RedirectToAction("Group", "Teacher", new { Id = id });
			return View(await _service.GetCreateAsync(id));
		}
		public async Task<IActionResult> Update(int groupid, DateTime date)
		{
			UpdateAttendanceVm vm = await _service.GetAttendanceUpdateAsync(groupid,date);
			if(vm ==null) return NotFound();
			return View(vm);
		}
		[HttpPost]
		public async Task<IActionResult> Update(int groupid,DateTime date,UpdateAttendanceVm vm)
		{
			if (await _service.AttendanceUpdateAsync(groupid, vm, ModelState)) 
				return RedirectToAction("Index",new {GroupId=groupid,Date=date});
			return View(await _service.GetAttendanceUpdateAsync(groupid,date));
		}
	}
}
