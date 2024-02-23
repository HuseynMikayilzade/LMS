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
	public interface IAttendanceService
	{
		Task<CreateAttendanceVm> GetCreateAsync(int groupid);
		Task<bool> CreateAsync(int groupid, CreateAttendanceVm vm , ModelStateDictionary modelstate);
		Task<List<Attendance>> GetAttendanceAsync(int groupid,DateTime date);
		Task<UpdateAttendanceVm> GetAttendanceUpdateAsync(int id, DateTime date);
		Task<bool> AttendanceUpdateAsync(int groupid, UpdateAttendanceVm vm, ModelStateDictionary modelstate);


	}
}
