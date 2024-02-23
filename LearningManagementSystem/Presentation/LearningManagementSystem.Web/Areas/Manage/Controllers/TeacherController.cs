using LearningManagementSystem.Application.Abstraction.Services;
using LearningManagementSystem.Application.ViewModels;
using LearningManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Web.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]

    public class TeacherController : Controller
    {
        private readonly ITeacherService _service;

        public TeacherController(ITeacherService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index(string search,int page = 1, int take = 10)
        {
            PaginationVm<Teacher> vm = await _service.GetAllAsync(false,page, take);
            if (vm.Items == null) return NotFound();
            return View(vm);
        }
        public async Task<IActionResult> Update(int id)
        {
            UpdateTeacherVm vm = new UpdateTeacherVm();
            vm = await _service.UpdatedAsync(id, vm);
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateTeacherVm vm)
        {
            if (await _service.UpdateAsync(id, vm, ModelState))
                return RedirectToAction(nameof(Index));
            return View(await _service.UpdatedAsync(id, vm));
        }
        public async Task<IActionResult> SoftDelete(int id)
        {
            await _service.SoftDeleteAsync(id);
            return Json(new { status = 200 });
        }
        public async Task<IActionResult> Delete(int id)
        { 
            await _service.DeleteAsync(id);
            return Json(new { status = 200 });
        }
        public async Task<IActionResult> Archive(int page = 1, int take = 10)
        {
            PaginationVm<Teacher> vm = await _service.GetAllAsync(true,page, take);
            if (vm.Items == null) return NotFound();
            return View(vm);
        }
        public async Task<IActionResult> Recovery(int id)
        {
            await _service.RecoveryAsync(id);
            return RedirectToAction(nameof(Archive));
        }
    }
}
