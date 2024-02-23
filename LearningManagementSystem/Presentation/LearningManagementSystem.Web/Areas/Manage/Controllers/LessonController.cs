using LearningManagementSystem.Application.Abstraction.Services;
using LearningManagementSystem.Application.ViewModels;
using LearningManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Web.Areas.Manage.Controllers
{
    [Area("manage")]
    public class LessonController : Controller
    {
        private readonly ILessonService _service;

        public LessonController(ILessonService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index(int page = 1, int take = 10)
        {
            PaginationVm<Lesson> vm = await _service.GetAllAsync(page, take);
            if (vm.Items == null) return NotFound();
            return View(vm);
        }
        public async Task<IActionResult> Create()
        {
            CreateLessonVm vm = new CreateLessonVm();
            vm = await _service.CreatedAsync(vm);
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateLessonVm vm)
        {
            if (await _service.CreateAsync(vm, ModelState))
                return RedirectToAction(nameof(Index));
            return View(await _service.CreatedAsync(vm));
        }
        public async Task<IActionResult> Update(int id)
        {
            UpdateLessonVm vm = new UpdateLessonVm();
            vm = await _service.UpdatedAsync(id, vm);
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateLessonVm vm)
        {
            if (await _service.UpdateAsync(id, vm, ModelState))
                return RedirectToAction(nameof(Index));
            return View(await _service.UpdatedAsync(id, vm));
        }
        public async Task<IActionResult> Delete(int id)
        {
           if(await _service.DeleteAsync(id))
                return RedirectToAction(nameof(Index));
            return NotFound();      
        }
    }
}