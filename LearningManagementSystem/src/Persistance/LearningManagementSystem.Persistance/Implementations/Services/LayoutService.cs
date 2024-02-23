using LearningManagementSystem.Application.Abstraction.Repositories;
using LearningManagementSystem.Application.Abstraction.Services;
using LearningManagementSystem.Application.Utilities.Exceptions;
using LearningManagementSystem.Application.ViewModels;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Persistance.Implementations.Services
{
    public class LayoutService
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITeacherRepo _teacherRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IGroupRepo _groupRepo;
        private readonly IStudentRepo _studentRepo;

        public LayoutService(SignInManager<AppUser> signInManager, ITeacherRepo teacherRepo,
            UserManager<AppUser> userManager, IGroupRepo groupRepo,IStudentRepo studentRepo)
        {
            _signInManager = signInManager;
            _teacherRepo = teacherRepo;
            _userManager = userManager;
            _groupRepo = groupRepo;
            _studentRepo = studentRepo;
        }

        public async Task<LayoutVm> GetUser(ClaimsPrincipal claims)
        {
            AppUser user = await _signInManager.UserManager.GetUserAsync(claims);
            if (user == null) throw new NotFoundException("Not found");
            if (await _userManager.IsInRoleAsync(user, Role.Teacher.ToString()))
            {
                LayoutVm vm = new LayoutVm
                {
                    user = user,
                    TeacherItemVm = await GetGroupsForTeacherAsync(user.Id)
                };
                return vm;
            }
            else
            {
                LayoutVm vm = new LayoutVm
                {
                    user = user,
                    StudentItemVm = await GetGroupForStudentAsync(user.Id)
                };
                return vm;
            }

        }
        public async Task<StudentItemVm> GetGroupForStudentAsync(string userid)
        {
            if (userid == null) throw new BadRequestException("Bad request");
            AppUser user = await _userManager.FindByIdAsync(userid);
            if (user == null) throw new NotFoundException("Not found");
            Student student = await _studentRepo.GetByExpressionAsync(x=>x.AppUserId==user.Id);
            if(student ==null) throw new NotFoundException("Not found");         
            Group group = await _groupRepo.GetByIdAsync((int)student.GroupId);
            if(group==null) throw new NotFoundException("Not found");
            StudentItemVm vm = new StudentItemVm
            {
                Group = group,
            };
            return vm;  
        }
        public async Task<TeacherItemVm> GetGroupsForTeacherAsync(string userid)
        {
            if (userid == null) throw new BadRequestException("Bad request");
            AppUser user = await _userManager.FindByIdAsync(userid);
            if (user == null) throw new NotFoundException("Not found");
            Teacher teacher = await _teacherRepo.GetByExpressionAsync(x => x.AppUserId == user.Id);
            ICollection<Group> groups = await _groupRepo.GetAllWhere(x => x.GroupTeachers != null && x.GroupTeachers
                           .Any(gt => gt.TeacherId == teacher.Id)).ToListAsync();
            if (groups == null) throw new NotFoundException("Not found");
            TeacherItemVm vm = new TeacherItemVm
            {
                Groups = groups
            };
            return vm;
        }
    }
}
