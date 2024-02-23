using LearningManagementSystem.Application.Abstraction.Services;
using LearningManagementSystem.Application.ViewModels;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Web.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles ="Admin")]
    public class GroupController : Controller
    {
        private readonly IGroupService _service;

        public GroupController(IGroupService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index(int page=1,int take=10)
        {      
            PaginationVm<Group> vm = await _service.GetAllAsync(page,take);
            if (vm == null) return NotFound();
            return View(vm);
        }
        public async Task<IActionResult> Create()
        {
            CreateGroupVm vm = new CreateGroupVm();
            vm = await _service.CreatedAsync(vm);
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateGroupVm vm)
        {
            if (await _service.CreateAsync(vm, ModelState))
                return RedirectToAction(nameof(Index));
            return View(await _service.CreatedAsync(vm)); 
        }

        public async Task<IActionResult> Update(int id)
        {
            UpdateGroupVm vm = new UpdateGroupVm();
            vm = await _service.UpdatedAsync(id,vm);
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,UpdateGroupVm vm)
        {
            if (await _service.UpdateAsync(id,vm, ModelState)) 
                return RedirectToAction(nameof(Index));
            return View(await _service.UpdatedAsync(id,vm));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if(result) return RedirectToAction(nameof(Index));
            return NotFound();
        }
    }
}
