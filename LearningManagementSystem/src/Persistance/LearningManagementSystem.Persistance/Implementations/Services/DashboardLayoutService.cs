using LearningManagementSystem.Application.Abstraction.Repositories;
using LearningManagementSystem.Application.Abstraction.Services;
using LearningManagementSystem.Application.Utilities.Exceptions;
using LearningManagementSystem.Application.ViewModels;
using LearningManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Persistance.Implementations.Services
{
    public class DashboardLayoutService : ISettingService
    {
        private readonly ISettingRepo _repo;

        public DashboardLayoutService(ISettingRepo repo)
        {
            _repo = repo;
        }

        public async Task<PaginationVm<Setting>> GetAllAsync(int page = 1, int take = 10)
        {
            if (page < 1 || take < 1) throw new BadRequestException("Bad request");
            IEnumerable<Setting> settings = await _repo.GetAllSetting();
            if (settings == null) throw new NotFoundException("Not found");
            int count = settings.Count();
            if (count < 0) throw new NotFoundException("Not found");
            double totalpage = Math.Ceiling((double)count / take);
            PaginationVm<Setting> vm = new PaginationVm<Setting>
            {
                Items = settings.ToList(),
                CurrentPage = page,
                TotalPage = totalpage
            };
            return vm;
        }

        public async Task<Dictionary<string, string>> GetSettings()
        {
            Dictionary<string, string> settings = await _repo.GetSettingsDictionary();
            return settings;
        }

        public async Task<bool> UpdateAsync(int id, UpdateSettingVm vm, ModelStateDictionary modelstate)
        {
            if (id < 1) throw new BadRequestException("Bad request");
            if (!modelstate.IsValid) return false;
            Setting exist = await _repo.GetSettingById(id);
            if (exist == null) throw new NotFoundException("Not found");
            exist.Value = vm.Value;
            exist.UpdateDate = DateTime.UtcNow;
            await _repo.Update(exist);
            return true;
        }

        public async Task<UpdateSettingVm> UpdatedAsync(int id, UpdateSettingVm vm)
        {
            if (id < 1) throw new BadRequestException("Bad request");
            Setting exist = await _repo.GetSettingById(id);
            if (exist == null) throw new NotFoundException("Not found");
            vm.Value = exist.Value;
            return vm;
        }
    }
}
