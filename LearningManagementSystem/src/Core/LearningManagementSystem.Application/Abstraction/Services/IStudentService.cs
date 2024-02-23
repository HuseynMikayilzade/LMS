
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
    public interface IStudentService
    {
        Task<bool> UpdateAsync(int id, UpdateStudentVm vm, ModelStateDictionary modelstate);
        Task<UpdateStudentVm> UpdatedAsync(int id, UpdateStudentVm vm);
        Task<StudentItemVm> GetGroupsAsync(string userid);
        Task<ICollection<Student>> GetAllWhereAsync(int id);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
        Task RecoveryAsync(int id);
        Task<PaginationVm<Student>> GetAllAsync(bool isdeleted, int page = 1, int take = 10);
        Task<UpdateStudentVm> GetStudentInfo(string userid, UpdateStudentVm vm);
        Task<UpdateStudentVm> StudentForEditAsync(string userid, UpdateStudentVm vm);
        Task<bool> StudentEditAsync(string userid, UpdateStudentVm vm, ModelStateDictionary modelstate);
    }
}
