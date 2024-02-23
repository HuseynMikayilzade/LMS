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
    public interface ILessonService
    {
        Task<bool> CreateAsync(CreateLessonVm vm, ModelStateDictionary modelstate);
        Task<CreateLessonVm> CreatedAsync(CreateLessonVm vm);
        Task<bool> UpdateAsync(int id,UpdateLessonVm vm, ModelStateDictionary modelstate);
        Task<UpdateLessonVm> UpdatedAsync(int id,UpdateLessonVm vm);
        Task<bool> DeleteAsync(int id);
        Task<PaginationVm<Lesson>> GetAllAsync(int page = 1,int take = 10);
    }
}
