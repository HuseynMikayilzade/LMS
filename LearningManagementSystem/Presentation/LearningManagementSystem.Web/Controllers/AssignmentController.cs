using LearningManagementSystem.Application.Abstraction.Services;
using LearningManagementSystem.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LearningManagementSystem.Web.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly IAssignmentService _service;

        public AssignmentController(IAssignmentService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> TaskResponse(int id)
        {        
            return View(await _service.GetStudentsResponsesAsync(id));
        }
        public async new Task<IActionResult> Response(int id)
        {
			var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			StudentResponseVm vm = await _service.GetStudentResponseAsync(id,userid);
            return View(vm);
        }
        [HttpPost]
        public async new Task<IActionResult> Response(int id, StudentResponseVm vm)
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (await _service.StudentResponseAsync(userid, id, vm, ModelState))
                return RedirectToAction(nameof(Response));
            return View(await _service.GetStudentResponseAsync(id,userid));
        }
        public async Task<IActionResult> Detail(int id)
        {
            TeacherResponseVm vm = await _service.ResponseDetailAsync(id);
            if (vm == null) return NotFound();
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Detail(int id, TeacherResponseVm vm)
        {
           if(await _service.TeacherResponseAsync(id,vm,ModelState))
                return RedirectToAction("TaskResponse",new {Id=vm.StudentResponse.AssignmentId});
            return Ok();
        }
        public async Task<IActionResult> Update(int id)
        {
            return View(await _service.GetTeacherResponseUpdate(id));
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,TeacherResponseVm vm)
        {
            if(await _service.TeacherResponseUpdate(id,vm,ModelState))
                return RedirectToAction("TaskResponse", new { Id = vm.StudentResponse.AssignmentId });
            return View(await _service.GetTeacherResponseUpdate(id));

        }
		public async  Task<IActionResult> UpdateResponse(int id)
		{
			UpdateStudenResponseVm vm = await _service.GetUpdateStudenResponse(id);
			return View(vm);
		}
		[HttpPost]
		public async  Task<IActionResult> UpdateResponse(int id, UpdateStudenResponseVm vm)
		{
            if (await _service.UpdateStudentResponse(id, vm, ModelState))
                return RedirectToAction("Response",new {Id=id});
			return View(await _service.GetUpdateStudenResponse(id));
		}
		
	}
}