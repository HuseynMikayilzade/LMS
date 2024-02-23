using LearningManagementSystem.Application.ViewModels;
using LearningManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Abstraction
{
    public interface ISubjectService
    {
        Task<bool> CreateAsync(CreateSubjectVm vm, ModelStateDictionary modelstate);
        Task<bool> UpdateAsync(int id, UpdateSubjectVm vm, ModelStateDictionary modelstate);
        Task<UpdateSubjectVm> UpdatedAsync(int id, UpdateSubjectVm vm);
        Task<bool> DeleteAsync(int id);
        Task<PaginationVm<Subject>> GetAllAsync(int page = 1, int take = 10);
    }
}
