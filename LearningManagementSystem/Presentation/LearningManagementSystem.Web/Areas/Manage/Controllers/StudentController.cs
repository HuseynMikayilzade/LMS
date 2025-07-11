﻿using LearningManagementSystem.Application.Abstraction.Services;
using LearningManagementSystem.Application.ViewModels;
using LearningManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Web.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]

    public class StudentController : Controller
    {
        private readonly IStudentService _service;

        public StudentController(IStudentService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index(string search,int page = 1, int take = 10)
        {
            PaginationVm<Student> vm = await _service.GetAllAsync(search,false, page, take);
            if (vm.Items == null) return NotFound();
            return View(vm);
        }
        public async Task<IActionResult> Update(int id)
        {
            UpdateStudentVm vm = new UpdateStudentVm();
            vm = await _service.UpdatedAsync(id, vm);
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateStudentVm vm)
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
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Archive(string search,int page = 1, int take = 10)
        {
            PaginationVm<Student> vm = await _service.GetAllAsync(search,true, page, take);
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
