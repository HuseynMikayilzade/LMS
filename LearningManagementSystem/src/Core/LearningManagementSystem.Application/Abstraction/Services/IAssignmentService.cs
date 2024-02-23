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
    public interface IAssignmentService
    {
        Task<bool> CreateAsync(CreateAssignmentVm vm, ModelStateDictionary modelstate);
        Task<bool> UpdateAsync(int id, UpdateAssignmentVm vm, ModelStateDictionary modelstate);
        Task<UpdateAssignmentVm> UpdatedAsync(int id, UpdateAssignmentVm vm);
        Task<bool> DeleteAsync(int id);
        Task ActivateAsync(int id);
        Task DeActivateAsync(int id);
        Task<StudentResponseVm> GetStudentResponseAsync(int id,string userid);
        Task<TeacherResponseVm> ResponseDetailAsync(int id);
        Task<UpdateStudenResponseVm> GetUpdateStudenResponse(int id);
        Task<bool> UpdateStudentResponse(int id,UpdateStudenResponseVm vm,ModelStateDictionary modelstate);   
		Task<ICollection<StudentResponse>> GetStudentsResponsesAsync(int id);
        Task<bool> StudentResponseAsync(string userid, int assignmentid, StudentResponseVm vm, ModelStateDictionary modelstate);
        Task<bool> TeacherResponseAsync(int id,TeacherResponseVm vm, ModelStateDictionary modelstate);
        Task<TeacherResponseVm> GetTeacherResponseUpdate(int id);
        Task<bool> TeacherResponseUpdate(int id, TeacherResponseVm vm, ModelStateDictionary modelstate);
    }
}
