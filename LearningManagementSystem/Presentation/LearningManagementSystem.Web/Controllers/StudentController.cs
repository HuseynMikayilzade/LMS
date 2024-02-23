using LearningManagementSystem.Application.Abstraction.Repositories;
using LearningManagementSystem.Application.Abstraction.Services;
using LearningManagementSystem.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LearningManagementSystem.Web.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _service;
        private readonly IGroupService _groupService;

        public StudentController(IStudentService service, IGroupService groupService)
        {
            _service = service;
            _groupService = groupService;
           
        }
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(await _service.GetGroupsAsync(userId));
        }
        public async Task<IActionResult> Group(int id,int attendancepage=1,int assignmentpage= 1)
        {
            GroupItemVm vm = await _groupService.GetGroupItems(id, attendancepage,assignmentpage);
            if (vm == null) return NotFound();
            return View(vm);
        }

        public async Task<IActionResult> Profile()
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            UpdateStudentVm vm = new UpdateStudentVm();
            vm = await _service.GetStudentInfo(userid, vm);
            return View(vm);
        }
        public async Task<IActionResult> Edit()
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            UpdateStudentVm vm = new UpdateStudentVm();
            vm = await _service.StudentForEditAsync(userid, vm);
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateStudentVm vm)
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (await _service.StudentEditAsync(userid, vm, ModelState))
                return RedirectToAction(nameof(Profile));
            return View(await _service.StudentForEditAsync(userid, vm));
        }
        

    }
}
