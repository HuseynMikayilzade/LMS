using LearningManagementSystem.Application.ViewModels;
using LearningManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Abstraction.Services
{
    public interface ISettingService
    {
        Task<bool> UpdateAsync(int id, UpdateSettingVm vm, ModelStateDictionary modelstate);
        Task<PaginationVm<Setting>> GetAllAsync(int page = 1, int take = 10);
        Task<Dictionary<string, string>> GetSettings();
        Task<UpdateSettingVm> UpdatedAsync(int id, UpdateSettingVm vm);
    }
}
