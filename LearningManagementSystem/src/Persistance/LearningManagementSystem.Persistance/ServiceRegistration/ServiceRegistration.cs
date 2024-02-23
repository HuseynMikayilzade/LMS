using LearningManagementSystem.Application.Abstraction;
using LearningManagementSystem.Application.Abstraction.Repositories;
using LearningManagementSystem.Application.Abstraction.Services;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Persistance.DAL;
using LearningManagementSystem.Persistance.Implementations;
using LearningManagementSystem.Persistance.Implementations.Repositories;
using LearningManagementSystem.Persistance.Implementations.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Persistance.ServiceRegistration
{
	public static class ServiceRegistration
	{
		public static IServiceCollection AddPersistanceService(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<AppDbContext>(opt =>
			{
				opt.UseSqlServer(configuration.GetConnectionString("mssql"));
			});
			services.AddIdentity<AppUser, IdentityRole>(opt =>
			{
				opt.User.RequireUniqueEmail = true;
				opt.Password.RequireNonAlphanumeric = false;
				opt.Password.RequiredLength = 8;
				opt.Lockout.AllowedForNewUsers = true;
				opt.Lockout.MaxFailedAccessAttempts = 10;
				opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);

			}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
			services.AddScoped<IGroupRepo, GroupRepository>();
			services.AddScoped<IGroupService, GroupService>();

			services.AddScoped<ILessonRepo, LessonRepository>();
			services.AddScoped<ILessonService, LessonService>();

			services.AddScoped<ISubjectRepo, SubjectRepository>();
			services.AddScoped<ISubjectService, SubjectService>();

			services.AddScoped<IUserService, UserService>();
			services.AddScoped<ISettingService, DashboardLayoutService>();
			services.AddScoped<ISettingRepo, SettingRepository>();
			services.AddScoped<LayoutService>();
			services.AddScoped<DashboardLayoutService>();

			services.AddScoped<ITeacherRepo, TeacherRepository>();
			services.AddScoped<ITeacherService, TeacherService>();

			services.AddScoped<IStudentRepo, StudentRepository>();
			services.AddScoped<IStudentService, StudentService>();

			services.AddScoped<IRoomRepo, RoomRepository>();
			services.AddScoped<IRoomService, RoomService>();

			services.AddScoped<IEmailService, EmailService>();
			services.AddScoped<IDashboardService, DashboardService>();

			services.AddScoped<IAssignmentRepo, AssignmentRepository>();
			services.AddScoped<IAssignmentService, AssignmentService>();

			services.AddScoped<IStudentResponseRepo, StudentResponseRepository>();
			services.AddScoped<ITeacherResponseRepo, TeacherResponseRepository>();

			services.AddScoped<IAttendanceRepo, AttendanceRepository>();
			services.AddScoped<IAttendanceService, AttendanceService>();

			return services;
		}

	}
}
