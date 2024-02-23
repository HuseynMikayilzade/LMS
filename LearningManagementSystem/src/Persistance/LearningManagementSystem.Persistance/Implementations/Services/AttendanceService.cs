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
	public class AttendanceService : IAttendanceService
	{
		private readonly IAttendanceRepo _repo;
		private readonly IStudentRepo _studentRepo;

		public AttendanceService(IAttendanceRepo repo, IStudentRepo studentRepo)
		{
			_repo = repo;
			_studentRepo = studentRepo;
		}

		public async Task<bool> CreateAsync(int groupid, CreateAttendanceVm vm, ModelStateDictionary modelstate)
		{
			if (groupid < 1) throw new BadRequestException("Bad request");
			if (!modelstate.IsValid) return false;
			if (await _repo.IsExist(x => x.Date == vm.Date))
			{
				modelstate.AddModelError(string.Empty, "Attendance is available on this date");
				return false;
			}
			for (int i = 0; i < vm.StudentIds.Count(); i++)
			{
				Student student = await _studentRepo.GetByIdAsync(vm.StudentIds[i]);
				if (student == null) throw new NotFoundException("Not found");
				if (vm.IsPresents[i] == false)
				{
					if (student.TotalAttendance < 15)
					{
						student.TotalAttendance++;
					}
					else
					{
						student.IsFailed = true;
					}
				}
				Attendance attendance = new Attendance
				{
					StudentId = student.Id,
					GroupId = groupid,
					Date = vm.Date,
					IsPresent = vm.IsPresents[i],
					Comment = vm.Comments[i]
				};
				await _repo.AddAsync(attendance);
			}
			await _repo.SaveChangesAsync();
			return true;
		}

		public async Task<CreateAttendanceVm> GetCreateAsync(int groupid)
		{
			if (groupid < 1) throw new BadRequestException("Bad request");
			List<Student> students = await _studentRepo.GetAllWhere(x => x.GroupId == groupid, includes: nameof(Group)).ToListAsync();
			if (students == null) throw new NotFoundException("Not found");
			List<AttendanceVm> attendancevm = new List<AttendanceVm>();
			foreach (var item in students)
			{
				attendancevm.Add(new AttendanceVm { Student = item, StudentId = item.Id });
			}
			CreateAttendanceVm vm = new CreateAttendanceVm
			{
				AttendanceVm = attendancevm,
			};
			return vm;
		}
		public async Task<List<Attendance>> GetAttendanceAsync(int groupid, DateTime date)
		{
			if (groupid < 1) throw new BadRequestException("Bad request");
			List<Attendance> attendances = await _repo.GetAllWhere(x => x.GroupId == groupid, includes: new string[] { nameof(Group), nameof(Student) }).ToListAsync();
			attendances = attendances.Where(x => x.Date == date).ToList();
			return attendances;
		}
		public async Task<UpdateAttendanceVm> GetAttendanceUpdateAsync(int id, DateTime date)
		{
			List<Attendance> attendances = await GetAttendanceAsync(id, date);
			List<AttendanceVm> attendanceVm = new List<AttendanceVm>();
			
			foreach (var item in attendances)
			{
				attendanceVm.Add(new AttendanceVm { Student = item.Student, StudentId = item.StudentId ,Comment = item.Comment,IsPresent=item.IsPresent});
			}
			UpdateAttendanceVm vm = new UpdateAttendanceVm
			{
				AttendanceVm = attendanceVm,			
				Date = date
			};
			return vm;
		}
		public async Task<bool> AttendanceUpdateAsync(int groupid,UpdateAttendanceVm vm ,ModelStateDictionary modelstate)
		{
			if (groupid < 1) throw new BadRequestException("Bad request");
			if (!modelstate.IsValid) return false;
			for (int i = 0; i < vm.StudentIds.Count(); i++)
			{
				Student student = await _studentRepo.GetByIdAsync(vm.StudentIds[i]);
				if (student == null) throw new NotFoundException("Not found");
				if (vm.IsPresents[i] == false)
				{
					if (student.TotalAttendance < 15)
					{
						student.TotalAttendance++;
					}
					else
					{
						student.IsFailed = true;
					}
				}
				Attendance exist = await _repo.GetByExpressionAsync(x => x.GroupId == groupid&&x.Date==vm.Date);
				exist.UpdateDate = DateTime.Now;
				exist.StudentId = vm.StudentIds[i];
				exist.IsPresent = vm.IsPresents[i];
				exist.Comment = vm.Comments[i];
				_repo.Update(exist);
			}
			await _repo.SaveChangesAsync();
			return true;
		}

	}
}
