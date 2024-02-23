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
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Persistance.Implementations.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepo _repo;
        private readonly ISubjectRepo _subjectRepo;
        private readonly IGroupRepo _groupRepo;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<AppUser> _userManager;
        private readonly IStudentRepo _studentRepo;
        private readonly IAssignmentRepo _assignmentRepo;

        public TeacherService(ITeacherRepo repo, ISubjectRepo subjectRepo,
            IGroupRepo groupRepo, IWebHostEnvironment env,
            UserManager<AppUser> userManager,IStudentRepo studentRepo,IAssignmentRepo assignmentRepo)
        {
            _repo = repo;
            _subjectRepo = subjectRepo;
            _groupRepo = groupRepo;
            _env = env;
            _userManager = userManager;
            _studentRepo = studentRepo;
            _assignmentRepo = assignmentRepo;
        }

        public async Task SoftDeleteAsync(int id)
        {
            if (id < 1) throw new BadRequestException("Bad request");
            Teacher exist = await _repo.GetByIdAsync(id);
            if (exist == null) throw new NotFoundException("Not found");
            exist.IsDeleted = true;
            _repo.Update(exist);
            await _repo.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            if (id < 1) throw new BadRequestException("Bad request");
            Teacher exist = await _repo.GetByIdAsync(id);
            if (exist == null) throw new NotFoundException("Not found");
            if (exist.Image != null && exist.Image != "profil.png")
            {
                exist.Image.DeleteFile(_env.WebRootPath, "assets", "img");
            }
            AppUser user = await _userManager.FindByEmailAsync(exist.EmailAddress);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            _repo.Delete(exist);
            await _repo.SaveChangesAsync();
        }
        public async Task<PaginationVm<Teacher>> GetAllAsync(bool isdeleted, int page = 1, int take = 10)
        {
            if (page < 1 || take < 1) throw new BadRequestException("Bad request");
            ICollection<Teacher> teachers = await _repo.GetAllWhere(x => x.IsDeleted == isdeleted, skip: (page - 1) * take, take: take, orderexpression: x => x.Id,
                isDescending: true, includes: new string[] { nameof(Subject), "GroupTeachers", "GroupTeachers.Group" }).ToListAsync();
            if (teachers == null) throw new NotFoundException("Not Found");
            int count = await _repo.GetAll().CountAsync();
            if (count < 0) throw new NotFoundException("Not Found");
            double totalpage = Math.Ceiling((double)count / take);
            PaginationVm<Teacher> vm = new PaginationVm<Teacher>
            {
                Items = teachers,
                CurrentPage = page,
                TotalPage = totalpage
            };
            return vm;
        }

        public async Task<bool> UpdateAsync(int id, UpdateTeacherVm vm, ModelStateDictionary modelstate)
        {
            if (id < 1) throw new BadRequestException("Bad request");
            if (!modelstate.IsValid) return false;
            if (!ValidateGender(vm.Gender, modelstate)) return false;           
            var exist = await _repo.GetByIdAsync(id, includes: new string[] { nameof(Subject), "GroupTeachers", "GroupTeachers.Group" });
            if (exist == null) throw new NotFoundException("Not found");

            if (vm.Photo != null)
            {
                var photoUpdateResult = await UpdatePhoto(vm.Photo, exist, modelstate);
                if (!photoUpdateResult) return false;
            }

            var groupUpdateResult = await UpdateGroups(vm.GroupIds, exist, modelstate);
            if (!groupUpdateResult) return false;

            UpdateTeacherProperties(vm, exist);
            await UpdateAppUser(exist.EmailAddress, exist);
            _repo.Update(exist);
            await _repo.SaveChangesAsync();
            return true;
        }

        private async Task<bool> UpdatePhoto(IFormFile photo, Teacher teacher, ModelStateDictionary modelstate)
        {
            if (photo.CheckSize(10))
            {
                modelstate.AddModelError("Photo", "Photo size incorrect");
                return false;
            }

            if (photo.CheckType())
            {
                modelstate.AddModelError("Photo", "Photo type incorrect");
                return false;
            }

            string filename = await photo.CreateFileAsync(_env.WebRootPath, "assets", "img");
            if (teacher.Image != null) teacher.Image.DeleteFile(_env.WebRootPath, "assets", "img");
            teacher.Image = filename;

            return true;
        }

        public async Task<UpdateTeacherVm> UpdatedAsync(int id, UpdateTeacherVm vm)
        {
            if (id < 1) throw new BadRequestException("Bad request");
            Teacher exist = await _repo.GetByIdAsync(id, includes: new string[] { nameof(Subject), "GroupTeachers", "GroupTeachers.Group" });
            if (exist == null) throw new NotFoundException("Not found");
            vm.Name = exist.Name;
            vm.Surname = exist.Surname;
            vm.PhoneNumber = exist.PhoneNumber;
            vm.Birthday = exist.Birthday;
            vm.Image = exist.Image;
            vm.Description = exist.Description;
            vm.Gender = exist.Gender;
            vm.SubjectId = exist.SubjectId;
            vm.Subjects = await _subjectRepo.GetAll().ToListAsync();
            vm.Groups = await _groupRepo.GetAll().ToListAsync();

            if (exist.GroupTeachers != null)
            {
                vm.GroupIds = exist.GroupTeachers.Select(gs => gs.GroupId).ToList();
            }
            else
            {
                vm.GroupIds = new List<int>();
            }
            return vm;
        }
        public async Task RecoveryAsync(int id)
        {
            if (id < 1) throw new BadRequestException("Bad request");
            Teacher exist = await _repo.GetByIdAsync(id);
            if (exist == null) throw new NotFoundException("Not found");
            exist.IsDeleted = false;
            _repo.Update(exist);
            await _repo.SaveChangesAsync();
        }

        public async Task<TeacherItemVm> GetGroupsAsync(string userid)
        {
            Teacher teacher = await GetTeacherByUserIdAsync(userid);
            ICollection<Group> groups = await _groupRepo.GetAllWhere(x => x.GroupTeachers != null && x.GroupTeachers.Any(gt => gt.TeacherId == teacher.Id),includes:new string[] { "GroupSubjects", "GroupSubjects.Subject" })
                           .ToListAsync();
            if (groups == null) throw new NotFoundException("Not found");
            var groupIds = groups.Select(g => g.Id).ToList();
            ICollection<Student> students = await _studentRepo.GetAllWhere(x => groupIds.Contains((int)x.GroupId)).ToListAsync();
            if (students == null) throw new NotFoundException("Not found");
            ICollection<Assignment> assignments = await _assignmentRepo.GetAllWhere(x => groupIds.Contains(x.GroupId)).ToListAsync();            
            if (assignments == null) throw new NotFoundException("Not found");
            
            TeacherItemVm vm = new TeacherItemVm
            {
                Groups = groups,
                Teacher = teacher,
                TotalStudent = students.Count,
                TotalTask =  assignments.Count,
                ActiveTask = assignments.Where(x=>x.IsActive==true).Count(),
            };
            return vm;
        }

        public async Task<UpdateTeacherVm> GetTeacherInfo(string userid, UpdateTeacherVm vm)
        {
            Teacher teacher = await GetTeacherByUserIdAsync(userid);
            vm.Email = teacher.EmailAddress;
            return await UpdatedAsync(teacher.Id, vm);
        }
       
        public async Task<UpdateTeacherVm> TeacherForEditAsync(string userid, UpdateTeacherVm vm)
        {
            Teacher teacher = await GetTeacherByUserIdAsync(userid);
            vm.Email = teacher.EmailAddress;
            vm.Birthday = teacher.Birthday;
            vm.Surname = teacher.Surname;
            vm.Name = teacher.Name;
            vm.PhoneNumber = teacher.PhoneNumber;
            vm.Gender = teacher.Gender;
            vm.Description = teacher.Description;
            vm.Image = teacher.Image;
            return vm;
        }
        public async Task<bool> TeacherEditAsync(string userid, UpdateTeacherVm vm, ModelStateDictionary modelstate)
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
            if (!ValidateGender(vm.Gender, modelstate)) return false;
            if (!StringFormatter.ValidateAge(vm.Birthday,20))
            {
                modelstate.AddModelError("Birthday", "You must be at least 20 years old.");
                return false;
            }
            Teacher exist = await GetTeacherByUserIdAsync(userid);
            if (vm.Photo != null)
            {
                var photoUpdateResult = await UpdatePhoto(vm.Photo, exist, modelstate);
                if (!photoUpdateResult) return false;
            }
            await UpdateAppUser(exist.EmailAddress, exist);
            UpdateTeacherProperties(vm,exist);
             _repo.Update(exist);
            await _repo.SaveChangesAsync();
            return true;
        }




        //////////////////// ========================= Private Methods =================== ////////////////////////
       
      
        private async Task<Teacher> GetTeacherByUserIdAsync(string userid)
        {
            if (userid == null) throw new BadRequestException("Bad request");
            AppUser user = await _userManager.FindByIdAsync(userid);
            if (user == null) throw new NotFoundException("Not Found");
            Teacher teacher = await _repo.GetByExpressionAsync(x => x.AppUserId == user.Id);
            if (teacher == null) throw new NotFoundException("Not found");
            return teacher;
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
        private async Task<bool> UpdateGroups(ICollection<int> groupIds, Teacher teacher, ModelStateDictionary modelstate)
        {
            if (teacher.GroupTeachers == null) teacher.GroupTeachers = new List<GroupTeacher>();
            if (groupIds != null)
            {
                foreach (var itemId in groupIds)
                {
                    if (!teacher.GroupTeachers.Any(gt => gt.GroupId == itemId))
                    {
                        if (!await _groupRepo.IsExist(c => c.Id == itemId))
                        {
                            modelstate.AddModelError(String.Empty, "This group isn't available");
                            return false;
                        }
                        teacher.GroupTeachers.Add(new GroupTeacher { GroupId = itemId });
                    }
                }
                teacher.GroupTeachers = teacher.GroupTeachers.Where(y => groupIds.Any(x => y.GroupId == x)).ToList();
            }
            else
            {
                teacher.GroupTeachers = new List<GroupTeacher>();
            }
            return true;
        }

        private void UpdateTeacherProperties(UpdateTeacherVm vm, Teacher teacher)
        {
            teacher.Name = vm.Name;
            teacher.Surname = vm.Surname;
            teacher.Description = vm.Description;
            teacher.PhoneNumber = vm.PhoneNumber;
            teacher.Birthday = vm.Birthday;
            teacher.SubjectId = vm.SubjectId;
            teacher.Gender = vm.Gender;
            teacher.UpdateDate = DateTime.UtcNow;
        }

        private async Task UpdateAppUser(string email, Teacher teacher)
        {
            if (!string.IsNullOrEmpty(email) && teacher != null)
            {
                AppUser user = await _userManager.FindByEmailAsync(email);           
                if (user == null) throw new NotFoundException("Not found");
                user.Name = teacher.Name;
                user.Surname = teacher.Surname;
                user.Gender = teacher.Gender;
                user.BirthDay = teacher.Birthday;
                user.PhoneNumber = teacher.PhoneNumber;
                user.Image = teacher.Image == null ? "profil.png" : teacher.Image;
                await _userManager.UpdateAsync(user);
            }
        }
    }
}
