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
    public interface ITeacherService
    {
        Task<bool> UpdateAsync(int id, UpdateTeacherVm vm, ModelStateDictionary modelstate);
        Task<UpdateTeacherVm> UpdatedAsync(int id, UpdateTeacherVm vm);
        Task<TeacherItemVm> GetGroupsAsync(string userid);
        Task DeleteAsync(int id);   
        Task RecoveryAsync(int id);
        Task SoftDeleteAsync(int id);
        Task<PaginationVm<Teacher>> GetAllAsync(bool isdeleted,int page = 1, int take = 10);
        Task<UpdateTeacherVm> GetTeacherInfo(string userid, UpdateTeacherVm vm);
        Task<UpdateTeacherVm> TeacherForEditAsync(string userid, UpdateTeacherVm vm);
        Task<bool> TeacherEditAsync(string userid, UpdateTeacherVm vm, ModelStateDictionary modelstate);
    }
}
