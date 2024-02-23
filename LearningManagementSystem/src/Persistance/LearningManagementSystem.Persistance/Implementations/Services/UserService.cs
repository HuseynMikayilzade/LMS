using LearningManagementSystem.Application.Abstraction.Repositories;
using LearningManagementSystem.Application.Abstraction.Services;
using LearningManagementSystem.Application.Utilities.Exceptions;
using LearningManagementSystem.Application.Utilities.Extentions;
using LearningManagementSystem.Application.ViewModels;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Persistance.Implementations.Services
{
	public class UserService : IUserService
	{
		private readonly IWebHostEnvironment _env;
		private readonly UserManager<AppUser> _usermanager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly ITeacherRepo _teacherRepo;
		private readonly IStudentRepo _studentRepo;

		public UserService(IWebHostEnvironment env, UserManager<AppUser> usermanager,
			RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager,
			ITeacherRepo teacherRepo, IStudentRepo studentRepo)
		{
			_env = env;
			_usermanager = usermanager;
			_roleManager = roleManager;
			_signInManager = signInManager;
			_teacherRepo = teacherRepo;
			_studentRepo = studentRepo;

		}
		public async Task CreateRoleAsync()
		{
			foreach (var item in Enum.GetValues(typeof(Role)))
			{
				if (!await _roleManager.RoleExistsAsync(item.ToString()))
				{
					await _roleManager.CreateAsync(new IdentityRole { Name = item.ToString() });
				}
			}
		}
		public async Task LogOutAsync()
		{
			await _signInManager.SignOutAsync();
		}

		private async Task ResetAsync(string userid)
		{
			AppUser user = await _usermanager.FindByIdAsync(userid);
			if (user == null) throw new NotFoundException("Not found");
			var resetToken = await _usermanager.GeneratePasswordResetTokenAsync(user);
			var result = await _usermanager.ResetPasswordAsync(user, resetToken, "Huseyn123");
			if (!result.Succeeded)
			{
				throw new BadRequestException("Bad request");
			}
			user.IsLogin = false;
			await _usermanager.UpdateAsync(user);
		}
		public async Task ResetPasswordDefaultTeacher(int id)
		{
			if (id < 1) throw new BadRequestException("Bad request");
			Teacher teacher = await _teacherRepo.GetByIdAsync(id);
			if (teacher == null) throw new NotFoundException("Not found");
			await ResetAsync(teacher.AppUserId);
		}
		public async Task ResetPasswordDefaultStudent(int id)
		{
			if (id < 1) throw new BadRequestException("Bad request");
			Student student = await _studentRepo.GetByIdAsync(id);
			if (student == null) throw new NotFoundException("Not found");
			await ResetAsync(student.AppUserId);
		}
		public async Task<bool> ResetProfilePassword(string userid, ResetPasswordVm vm, ModelStateDictionary modelstate)
		{
			if (!modelstate.IsValid) return false;
			if (string.IsNullOrEmpty(userid))
			{
				modelstate.AddModelError(string.Empty, "User not found");
				return false;
			}
			AppUser user = await _usermanager.FindByIdAsync(userid);
			if (user == null)
			{
				modelstate.AddModelError(string.Empty, "User not found");
				return false;
			}
			if (vm.OldPassword == vm.NewPassword)
			{
				modelstate.AddModelError(string.Empty, "New Password cannot be the same as the Old Password");
				return false;
			}
			var result = await _usermanager.CheckPasswordAsync(user, vm.OldPassword);
			if (!result)
			{
				modelstate.AddModelError(string.Empty, "Old Password incorrect");
				return false;
			}
			var changeresult = await _usermanager.ChangePasswordAsync(user, vm.OldPassword, vm.NewPassword);
			if (!changeresult.Succeeded)
			{
				foreach (var item in changeresult.Errors)
				{
					modelstate.AddModelError(string.Empty, item.Description);
					return false;
				}
			}
			return true;
		}
		public async Task<bool> LoginAsync(LoginVm vm, ModelStateDictionary modelstate)
		{
			if (!modelstate.IsValid) return false;
			AppUser user = await _usermanager.FindByEmailAsync(vm.Email);
			if (user == null)
			{
				modelstate.AddModelError(string.Empty, "Not found");
				return false;
			}
			var result = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.IsRemembered, true);
			if (result.IsLockedOut)
			{
				modelstate.AddModelError(string.Empty, "Account is locked. Please try again after a few minutes.");
				return false;
			}
			if (!result.Succeeded)
			{
				modelstate.AddModelError(string.Empty, "Password or email incorrect");
				return false;
			}

			return true;
		}
		public async Task<bool> IsLoginAsync(string email)
		{
			AppUser user = await _usermanager.FindByEmailAsync(email);
			if (user == null) throw new NotFoundException("Not found");
			if (user.IsLogin == true)
			{
				return true;
			}
			return false;
		}
		public async Task<bool> FirstLoginForResetPasswordAsync(ResetPasswordVm vm, string username, ModelStateDictionary modelstate)
		{
			if (!modelstate.IsValid) return false;
			if (string.IsNullOrEmpty(username)) throw new NotFoundException("Not found");
			AppUser user = await _usermanager.FindByNameAsync(username);
			if (user == null)
			{
				modelstate.AddModelError(string.Empty, "Not found");
				return false;
			}
			var resettoken = await _usermanager.GeneratePasswordResetTokenAsync(user);
			var result = await _usermanager.ResetPasswordAsync(user, resettoken, vm.ConfirmPassword);
			if (!result.Succeeded)
			{
				modelstate.AddModelError(string.Empty, "Password reset failed. Please try again.");
				return false;
			}
			user.IsLogin = true;
			await _usermanager.UpdateAsync(user);
			await _signInManager.SignInAsync(user, isPersistent: false);
			return true;
		}
		public async Task<bool> RegisterAsync(RegisterVm vm, ModelStateDictionary modelstate)
		{
			if (!modelstate.IsValid) return false;
			if (vm.Name.isDigit())
			{
				modelstate.AddModelError("Name", "Name cannot contain numbers");
				return false;
			}
			if (vm.Surname.isDigit())
			{
				modelstate.AddModelError("Surname", "Surname cannot contain numbers");
				return false;
			}
			if (!ValidateEmail(vm.EmailAdress, modelstate)) return false;
			if (!ValidateGender(vm.Gender, modelstate)) return false;
			if (!ValidateRole(vm.Role, modelstate)) return false;
			var user = await CreateUserAsync(vm, modelstate);
			if (user == null) return false;

			var result = await _usermanager.CreateAsync(user, vm.Password);
			if (!result.Succeeded)
			{
				foreach (var item in result.Errors)
				{
					modelstate.AddModelError(string.Empty, item.Description);
				}
				return false;
			}

			await HandleUserRoleAsync(user, vm.Role);

			return true;
		}
		private bool ValidateEmail(string email, ModelStateDictionary modelstate)
		{
			if (!email.CheckEmail())
			{
				modelstate.AddModelError("EmailAdress", "Email is not entered correctly");
				return false;
			}
			return true;
		}
		private bool ValidateGender(Gender gender, ModelStateDictionary modelstate)
		{
			if (!Enum.IsDefined(typeof(Gender), gender))
			{
				modelstate.AddModelError(string.Empty, "Please select a valid gender");
				return false;
			}
			return true;
		}
		private bool ValidateRole(Role role, ModelStateDictionary modelstate)
		{
			if (!Enum.IsDefined(typeof(Role), role))
			{
				modelstate.AddModelError(string.Empty, "Please select a valid role");
				return false;
			}
			return true;
		}
		private async Task<AppUser> CreateUserAsync(RegisterVm vm, ModelStateDictionary modelstate)
		{
			var user = new AppUser
			{
				UserName = vm.Name.Capitalize() + vm.Surname.Capitalize(),
				Name = vm.Name.Capitalize(),
				Surname = vm.Surname.Capitalize(),
				Email = vm.EmailAdress,
				PhoneNumber = vm.PhoneNumber,
				BirthDay = vm.BirthDay,
				Gender = vm.Gender,
				Role = vm.Role
			};

			if (vm.Photo != null)
			{
				if (vm.Photo.CheckSize(10))
				{
					modelstate.AddModelError("Photo", "Photo size incorrect");
					return null;
				}
				if (vm.Photo.CheckType())
				{
					modelstate.AddModelError("Photo", "Photo type incorrect");
					return null;
				}
				string filename = await vm.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img");
				user.Image = filename;
			}
			return user;
		}
		private async Task HandleUserRoleAsync(AppUser user, Role role)
		{

			await _usermanager.AddToRoleAsync(user, role.ToString());

			if (role == Role.Teacher)
			{
				var teacher = new Teacher
				{
					AppUserId = user.Id,
					Surname = user.Surname,
					EmailAddress = user.Email,
					Name = user.Name,
					PhoneNumber = user.PhoneNumber,
					Gender = user.Gender,
					Birthday = user.BirthDay,
					Image = user.Image,
				};
				await _teacherRepo.AddAsync(teacher);
				await _teacherRepo.SaveChangesAsync();
			}
			else if (role == Role.Student)
			{
				var student = new Student
				{
					AppUserId = user.Id,
					Surname = user.Surname,
					EmailAddress = user.Email,
					Name = user.Name,
					PhoneNumber = user.PhoneNumber,
					Gender = user.Gender,
					Birthday = user.BirthDay,
					Image = user.Image,
				};
				await _studentRepo.AddAsync(student);
				await _studentRepo.SaveChangesAsync();
			}
		}
	}
}

