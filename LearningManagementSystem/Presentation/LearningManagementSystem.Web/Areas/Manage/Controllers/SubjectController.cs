using LearningManagementSystem.Application.Abstraction;
using LearningManagementSystem.Application.ViewModels;
using LearningManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Web.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]

    public class SubjectController : Controller
    {
        private readonly ISubjectService _service;

        public SubjectController(ISubjectService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index(int page = 1,int take =10)
        {
            PaginationVm<Subject> vm = await _service.GetAllAsync(page,take);
            if (vm.Items == null) return NotFound();
            return View(vm);
        }
        public IActionResult Create()
        {       
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSubjectVm vm)
        {
            if (await _service.CreateAsync(vm, ModelState))
                return RedirectToAction(nameof(Index));
            return View(vm);
        }
        public async Task<IActionResult> Update(int id)
        {
            UpdateSubjectVm vm = new UpdateSubjectVm();
            vm = await _service.UpdatedAsync(id, vm);
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateSubjectVm vm)
        {
            if (await _service.UpdateAsync(id, vm, ModelState))
                return RedirectToAction(nameof(Index));
            return View(await _service.UpdatedAsync(id, vm));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (result) return RedirectToAction(nameof(Index));
            return NotFound();
        }
    }
}
