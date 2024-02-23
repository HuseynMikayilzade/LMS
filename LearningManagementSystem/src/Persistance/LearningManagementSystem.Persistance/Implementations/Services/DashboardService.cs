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
    public class DashboardService:IDashboardService
    {
        private readonly IGroupRepo _groupRepo;
        private readonly ITeacherRepo _teacherRepo;
        private readonly IStudentRepo _studentRepo;

        public DashboardService(IGroupRepo groupRepo,ITeacherRepo teacherRepo,IStudentRepo studentRepo)
        {
            _groupRepo = groupRepo;
            _teacherRepo = teacherRepo;
            _studentRepo = studentRepo;
        }

        public async Task<DashboardVm> GetAllAsync()
        {
            int groupcount = await _groupRepo.GetAll().CountAsync();
            int teachercount = await _teacherRepo.GetAll().CountAsync();
            int studentcount = await _studentRepo.GetAll().CountAsync();
            ICollection<Student> starstudents = await _studentRepo.GetAllWhere(orderexpression: x => x.Avarage, isDescending: true,take:5).ToListAsync();
            if(starstudents==null) throw new NotFoundException("Not found");
            DashboardVm vm = new DashboardVm
            {
                GroupCount = groupcount,
                TeacherCount = teachercount,
                StudentCount = studentcount,
                StarStudents = starstudents
            };
            return vm;
        }
    }
}
