using LearningManagementSystem.Application.Abstraction.Services;
using LearningManagementSystem.Application.ViewModels;
using LearningManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Web.Areas.Manage.Controllers
{
    [Area("manage")]
    public class SettingController : Controller
    {
        private readonly ISettingService _service;

        public SettingController(ISettingService service )
        {
            _service = service;
        }
        public async Task<IActionResult> Index(int page=1,int take=1)
        {
            PaginationVm<Setting> vm = await _service.GetAllAsync();
            if (vm == null) return NotFound();
            return View(vm);
        }
        public async Task<IActionResult> Update(int id)
        {
            UpdateSettingVm vm = new UpdateSettingVm();
            return View(await _service.UpdatedAsync(id,vm));
        }
    }
}
