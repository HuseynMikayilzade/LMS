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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Persistance.Implementations.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepo _repo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _env;
        private readonly IGroupRepo _groupRepo;

        public StudentService(IStudentRepo repo, UserManager<AppUser> userManager, IWebHostEnvironment env, IGroupRepo groupRepo)
        {
            _repo = repo;
            _userManager = userManager;
            _env = env;
            _groupRepo = groupRepo;
        }
        public async Task SoftDeleteAsync(int id)
        {
            if (id < 1) throw new BadRequestException("Bad request");
            Student exist = await _repo.GetByIdAsync(id);
            if (exist == null) throw new NotFoundException("Not found");
            exist.IsDeleted = true;
            _repo.Update(exist);
            await _repo.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            if (id < 1) throw new BadRequestException("Bad request");
            Student exist = await _repo.GetByIdAsync(id);
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
            Group group = await _groupRepo.GetByExpressionAsync(x=>x.Id==exist.GroupId);
            if (group == null) throw new NotFoundException("Not found");
            group.CurrentStudent--;
            _repo.Delete(exist);
            await _repo.SaveChangesAsync();
        }
        public async Task RecoveryAsync(int id)
        {
            if (id < 1) throw new BadRequestException("Bad request");
            Student exist = await _repo.GetByIdAsync(id);
            if (exist == null) throw new NotFoundException("Not found");
            exist.IsDeleted = false;
            _repo.Update(exist);
            await _repo.SaveChangesAsync();
        }
        public async Task<PaginationVm<Student>> GetAllAsync(string seacrh,bool isdeleted, int page = 1, int take = 10)
        {
            if (page < 1 || take < 1) throw new BadRequestException("Bad request");
            IQueryable<Student> students =  _repo.GetAllWhere(x => x.IsDeleted == isdeleted, skip: (page - 1) * take, take: take, orderexpression: x => x.Id,
                isDescending: true, includes: new string[] { nameof(Group) }).AsQueryable();
            if (students == null) throw new NotFoundException("Not found");
            int count = await _repo.GetAll().CountAsync();
            if (count < 0) throw new NotFoundException("Not found");
            double totalpage = Math.Ceiling((double)count / take);
            if (!String.IsNullOrEmpty(seacrh))
            {
                students = students.Where(x => x.Name.ToLower().Contains(seacrh.ToLower()) || x.Surname.ToLower().Contains(seacrh.ToLower()));
            }
            PaginationVm<Student> vm = new PaginationVm<Student>
            {
                Items =await students.ToListAsync(),
                CurrentPage = page,
                TotalPage = totalpage
            };
            return vm;
        }
        private async Task UpdateAppUser(string email, Student student)
        {
            if (!string.IsNullOrEmpty(email) && student != null)
            {
                AppUser user = await _userManager.FindByEmailAsync(email);
                if (user == null) throw new NotFoundException("Not found");
                user.Name = student.Name;
                user.Surname = student.Surname;
                user.Gender = student.Gender;
                user.BirthDay = student.Birthday;
                user.PhoneNumber = student.PhoneNumber;
                user.Image = student.Image == null ? "profil.png" : student.Image;
                await _userManager.UpdateAsync(user);
            }
        }
        private async Task<bool> UpdatePhoto(IFormFile photo, Student student, ModelStateDictionary modelstate)
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
            if (student.Image != null) student.Image.DeleteFile(_env.WebRootPath, "assets", "img");
            student.Image = filename;
            return true;
        }
        public async Task<bool> UpdateAsync(int id, UpdateStudentVm vm, ModelStateDictionary modelstate)
        {
            if (id < 1) throw new BadRequestException("Bad request");
            if (!modelstate.IsValid) return false;
            if (!ValidateGender(vm.Gender, modelstate)) return false;
            var exist = await _repo.GetByIdAsync(id, includes: nameof(Group));
            if (exist == null) throw new NotFoundException("Not found");

            if (vm.Photo != null)
            {
                var photoUpdateResult = await UpdatePhoto(vm.Photo, exist, modelstate);
                if (!photoUpdateResult) return false;
            }
            Group existingGroup = null;
            if (exist.GroupId != null)
            {
                existingGroup = await _groupRepo.GetByIdAsync((int)exist.GroupId);
                if (existingGroup == null) throw new NotFoundException("Current group not found");
             
            }           
            Group newGroup = await _groupRepo.GetByIdAsync(vm.GroupId);
            if (newGroup == null) throw new NotFoundException("New group not found");
            if (existingGroup != null)
            {
                 existingGroup.CurrentStudent--;
                 _groupRepo.Update(existingGroup);
            }          
            newGroup.CurrentStudent++;
            _groupRepo.Update(newGroup);  
            exist.GroupId = newGroup.Id;
            UpdateStudentProperties(vm, exist);
            await UpdateAppUser(exist.EmailAddress, exist);
            _repo.Update(exist);
            await _repo.SaveChangesAsync();
            return true;
        }
        private void UpdateStudentProperties(UpdateStudentVm vm, Student student)
        {
            student.Name = vm.Name;
            student.Surname = vm.Surname;
            student.PhoneNumber = vm.PhoneNumber;
            student.Birthday = vm.Birthday;
            student.Gender = vm.Gender;
            student.UpdateDate = DateTime.UtcNow;
        }

        public async Task<UpdateStudentVm> UpdatedAsync(int id, UpdateStudentVm vm)
        {
            if (id < 1) throw new BadRequestException("Bad request");
            var exist = await _repo.GetByIdAsync(id, includes: nameof(Group));
            if (exist == null) throw new NotFoundException("Not found");
            vm.Name = exist.Name;
            vm.PhoneNumber = exist.PhoneNumber;
            vm.Birthday = exist.Birthday;
            if(exist.GroupId!=null) vm.GroupId = (int)exist.GroupId;
            vm.Gender = exist.Gender;
            vm.Surname = exist.Surname;
            vm.Image = exist.Image;
            vm.TotalAttendance = exist.TotalAttendance;
            vm.IsGraduated = exist.IsGraduated;
            vm.IsFailed = exist.IsFailed;
            vm.Groups = await _groupRepo.GetAllWhere(x=>x.CurrentStudent<x.MaxStudent).ToListAsync();
            return vm;
        }
        public async Task<StudentItemVm> GetGroupsAsync(string userid)
        {
            Student student = await GetStudentByUserIdAsync(userid);
            Group group = await _groupRepo.GetByIdAsync((int)student.GroupId);
            if (group == null) throw new NotFoundException("Not found");
            StudentItemVm vm = new StudentItemVm
            {
                Group = group
            };
            return vm;
        }

        public async Task<ICollection<Student>> GetAllWhereAsync(int id)
        {
            if (id < 1) throw new BadRequestException("Bad request");
            ICollection<Student> students = await _repo.GetAllWhere(x => x.GroupId == id).ToListAsync();
            if (students == null) throw new NotFoundException("Not found");
            return students;
        }



        public async Task<UpdateStudentVm> GetStudentInfo(string userid, UpdateStudentVm vm)
        {
            Student student = await GetStudentByUserIdAsync(userid);
            vm.Email = student.EmailAddress;
            vm.Point = student.Point;
            return await UpdatedAsync(student.Id,vm);
        }

        public async Task<UpdateStudentVm> StudentForEditAsync(string userid, UpdateStudentVm vm)
        {
            Student student = await GetStudentByUserIdAsync(userid);
            vm.Email = student.EmailAddress;
            vm.Birthday = student.Birthday;
            vm.Surname = student.Surname;
            vm.Name = student.Name;
            vm.PhoneNumber = student.PhoneNumber;
            vm.Gender = student.Gender;         
            vm.Image = student.Image;
            return vm;
        }

        public async Task<bool> StudentEditAsync(string userid, UpdateStudentVm vm, ModelStateDictionary modelstate)
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
            if (!StringFormatter.ValidateAge(vm.Birthday,5))
            {
                modelstate.AddModelError("Birthday", "You must be at least 5 years old.");
                return false;
            }
            Student exist = await GetStudentByUserIdAsync(userid);
            if (vm.Photo != null)
            {
                var photoUpdateResult = await UpdatePhoto(vm.Photo, exist, modelstate);
                if (!photoUpdateResult) return false;
            }
            await UpdateAppUser(exist.EmailAddress, exist);
            UpdateStudentProperties(vm,exist);
            _repo.Update(exist);
            await _repo.SaveChangesAsync();
            return true;
        }





        //////////////////// ========================= Private Methods =================== ////////////////////////
        private bool ValidateGender(Gender gender, ModelStateDictionary modelstate)
        {
            if (!Enum.IsDefined(typeof(Gender), gender))
            {
                modelstate.AddModelError(string.Empty, "Please select a valid gender");
                return false;
            }
            return true;
        }
        private async Task<Student> GetStudentByUserIdAsync(string userid)
        {
            if (userid == null) throw new BadRequestException("Bad request");
            AppUser user = await _userManager.FindByIdAsync(userid);
            if (user == null) throw new NotFoundException("Not found");
            Student student = await _repo.GetByExpressionAsync(x => x.AppUserId == user.Id);
            if (student == null) throw new NotFoundException("Not found");
            return student;
        }

        
    }
}
