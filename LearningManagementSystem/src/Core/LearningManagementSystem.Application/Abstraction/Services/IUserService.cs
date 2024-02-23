using LearningManagementSystem.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Abstraction.Services
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(RegisterVm vm, ModelStateDictionary modelstate);
        Task<bool> LoginAsync(LoginVm vm, ModelStateDictionary modelstate);
        Task LogOutAsync();
        Task CreateRoleAsync();
        Task<bool> IsLoginAsync(string email);
        Task ResetPasswordDefaultTeacher(int id);
		Task ResetPasswordDefaultStudent(int id);
		Task<bool> FirstLoginForResetPasswordAsync(ResetPasswordVm vm, string username, ModelStateDictionary modelstate);
        Task<bool> ResetProfilePassword(string userid, ResetPasswordVm vm, ModelStateDictionary modelstate);
		//Task<bool> ForgetPassword(ForgotPasswordVm vm, ModelStateDictionary modelstate);

	}
}
