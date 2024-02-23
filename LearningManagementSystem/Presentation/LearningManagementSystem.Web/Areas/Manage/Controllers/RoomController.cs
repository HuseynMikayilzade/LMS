using LearningManagementSystem.Application.Abstraction.Services;
using LearningManagementSystem.Application.ViewModels;
using LearningManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Web.Areas.Manage.Controllers
{
    [Area("manage")]
    public class RoomController : Controller
    {
        private readonly IRoomService _service;

        public RoomController(IRoomService roomService)
        {
            _service = roomService;
        }
        public async Task<IActionResult> Index(int page=1,int take=10)
        {
            PaginationVm<Room> vm = await _service.GetAllAsync(page, take);
            if (vm.Items == null) return NotFound();
            return View(vm);
        }
        public  IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateRoomVm vm)
        {
            if (await _service.CreateAsync(vm, ModelState))
                return RedirectToAction(nameof(Index));
            return View(vm);
        }
        public IActionResult Update(int id)
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateRoomVm vm)
        {
            if (await _service.UpdateAsync(id, vm, ModelState))
                return RedirectToAction(nameof(Index));
            return View(vm);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (result) return RedirectToAction(nameof(Index));
            return NotFound();
        }
    }
}
