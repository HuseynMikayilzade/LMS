using LearningManagementSystem.Application.Abstraction.Repositories;
using LearningManagementSystem.Application.Abstraction.Services;
using LearningManagementSystem.Application.Utilities.Exceptions;
using LearningManagementSystem.Application.Utilities.Extentions;
using LearningManagementSystem.Application.ViewModels;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Persistance.Implementations.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IAssignmentRepo _repo;
        private readonly IWebHostEnvironment _env;
        private readonly IStudentResponseRepo _studentResponseRepo;
        private readonly IStudentRepo _studentRepo;
        private readonly ITeacherResponseRepo _teacherResponseRepo;
        private readonly UserManager<AppUser> _userManager;

        public AssignmentService(IAssignmentRepo repo, IWebHostEnvironment env,
            IStudentResponseRepo studentResponseRepo, IStudentRepo studentRepo,
            ITeacherResponseRepo teacherResponseRepo, UserManager<AppUser> userManager)
        {
            _repo = repo;
            _env = env;
            _studentResponseRepo = studentResponseRepo;
            _studentRepo = studentRepo;
            _teacherResponseRepo = teacherResponseRepo;
            _userManager = userManager;
        }
        public async Task<bool> CreateAsync(CreateAssignmentVm vm, ModelStateDictionary modelstate)
        {
            if (!modelstate.IsValid) return false;
            if (vm.StartTime > vm.EndTime)
            {
                modelstate.AddModelError(string.Empty, "End Date must be greater than Start Date");
                return false;
            }
            if (vm.MaxPoint > 100 || vm.MaxPoint < 0)
            {
                modelstate.AddModelError(string.Empty, "minimum 0 and maximum 100 points");
                return false;
            }
            if (!Enum.IsDefined(typeof(TaskType), vm.TaskType))
            {
                modelstate.AddModelError(string.Empty, "Please select a valid type");
                return false;
            }
            Assignment assignment = new Assignment
            {
                IsActive = vm.IsActive,
                Name = vm.Name,
                CreateDate = DateTime.Now,
                Description = vm.Description,
                EndTime = vm.EndTime,
                StartTime = vm.StartTime,
                MaxPoint = vm.MaxPoint,
                TaskType = vm.TaskType,
                GroupId = (int)vm.GroupId
            };
            await _repo.AddAsync(assignment);
            await _repo.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            if (id < 1) throw new BadRequestException("Bad request");
            Assignment exist = await _repo.GetByIdAsync(id);
            if (exist == null) throw new NotFoundException("Not found");
            _repo.Delete(exist);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(int id, UpdateAssignmentVm vm, ModelStateDictionary modelstate)
        {
            if (id < 1) throw new BadRequestException("Bad request");
            Assignment exist = await _repo.GetByIdAsync(id);
            if (exist == null) throw new NotFoundException("Not found");
            if (vm.StartTime > vm.EndTime)
            {
                modelstate.AddModelError("StartTime", "End Date must be greater than Start Date");
                return false;
            }
            if (vm.MaxPoint > 100 || vm.MaxPoint < 0)
            {
                modelstate.AddModelError("MaxPoint", "minimum 0 and maximum 100 points");
                return false;
            }
            if (!Enum.IsDefined(typeof(TaskType), vm.TaskType))
            {
                modelstate.AddModelError(string.Empty, "Please select a valid type");
                return false;
            }
            exist.StartTime = vm.StartTime;
            exist.EndTime = vm.EndTime;
            exist.MaxPoint = vm.MaxPoint;
            exist.TaskType = vm.TaskType;
            exist.UpdateDate = DateTime.Now;
            exist.Description = vm.Description;
            exist.IsActive = vm.IsActive;
            exist.Name = vm.Name;
            _repo.Update(exist);
            await _repo.SaveChangesAsync();
            return true;
        }
        public async Task<UpdateAssignmentVm> UpdatedAsync(int id, UpdateAssignmentVm vm)
        {
            if (id < 1) throw new BadRequestException("Bad request");
            Assignment exist = await _repo.GetByIdAsync(id);
            if (exist == null) throw new NotFoundException("Not found");
            vm.Description = exist.Description;
            vm.IsActive = exist.IsActive;
            vm.Name = exist.Name;
            vm.TaskType = exist.TaskType;
            vm.StartTime = exist.StartTime;
            vm.EndTime = exist.EndTime;
            vm.MaxPoint = exist.MaxPoint;
            return vm;
        }
        public async Task ActivateAsync(int id)
        {
            if (id < 1) throw new BadRequestException("Bad request");
            Assignment exist = await _repo.GetByIdAsync(id);
            if (exist == null) throw new NotFoundException("Not found");
            exist.IsActive = true;
            _repo.Update(exist);
            await _repo.SaveChangesAsync();
        }
        public async Task DeActivateAsync(int id)
        {
            if (id < 1) throw new BadRequestException("Bad request");
            Assignment exist = await _repo.GetByIdAsync(id);
            if (exist == null) throw new NotFoundException("Not found");
            exist.IsActive = false;
            _repo.Update(exist);
            await _repo.SaveChangesAsync();
        }

        public async Task<StudentResponseVm> GetStudentResponseAsync(int id)
        {
            if (id < 1) throw new BadRequestException("Bad request");
            Assignment assignment = await _repo.GetByIdAsync(id);
            if (assignment == null) throw new NotFoundException("Not found");
            StudentResponse studentResponse = await _studentResponseRepo.GetByExpressionAsync(x => x.AssignmentId == id);
            if (studentResponse != null)
            {
                StudentResponseVm vm = new StudentResponseVm
                {
                    Assignment = assignment,
                    IsSended = studentResponse.IsSended,
                    IsReplied = studentResponse.IsReplied,
                };
                if (vm.IsReplied)
                {
                    TeacherResponse teacherResponse = await _teacherResponseRepo.GetByExpressionAsync(x => x.StudentId == studentResponse.StudentId);
                    vm.TeacherNote = teacherResponse.Note;
                    vm.Point = teacherResponse.Point;
                }
                return vm;
            }
            else
            {
                StudentResponseVm vm = new StudentResponseVm
                {
                    Assignment = assignment,                   
                };
                return vm;
            }
           
        }
        public async Task<bool> StudentResponseAsync(string userid, int assignmentid, StudentResponseVm vm, ModelStateDictionary modelstate)
        {
            if (!modelstate.IsValid) return false;
            if (vm.Response == null && vm.AttachedFile == null)
            {
                modelstate.AddModelError(String.Empty, "The answer cannot be empty");
                return false;
            }
            if (userid == null) throw new BadRequestException("Bad request");
            AppUser user = await _userManager.FindByIdAsync(userid);
            if (user == null) throw new NotFoundException("Not found");
            Student student = await _studentRepo.GetByExpressionAsync(x => x.AppUserId == user.Id);
            if (student == null) throw new NotFoundException("Not found");
            StudentResponse response = new StudentResponse
            {
                StudentId = student.Id,
                AssignmentId = assignmentid,
                CreateDate = DateTime.Now,               
            };
            if (vm.AttachedFile != null)
            {
                if (vm.AttachedFile.CheckSize(20))
                {
                    modelstate.AddModelError("AttachedFile", "Maximum file size is 20 mb");
                    return false;
                }
                string file = await vm.AttachedFile.CreateFileAsync(_env.WebRootPath, "uploads");
                response.AttachedFilePath = file;
                response.IsSended = true;
            }
            if (vm.Response != null)
            {
                response.Response = vm.Response;
                response.IsSended = true;
			}
            await _studentResponseRepo.AddAsync(response);
            await _studentResponseRepo.SaveChangesAsync();
            return true;
        }
        public async Task<ICollection<StudentResponse>> GetStudentsResponsesAsync(int id)
        {
            if (id < 1) throw new BadRequestException("Bad requset");
            Assignment assignment = await _repo.GetByIdAsync(id);
            if (assignment == null) throw new NotFoundException("Not Found");
            ICollection<StudentResponse> studentResponses = await _studentResponseRepo.GetAllWhere(x => x.AssignmentId == id, includes: new string[] { nameof(Student), nameof(Assignment) }).ToListAsync();
            if (studentResponses == null) throw new NotFoundException("Not found");
            return studentResponses;
        }


        public async Task<TeacherResponseVm> ResponseDetailAsync(int id)
        {
            if (id < 1) throw new BadRequestException("Bad request");
            StudentResponse studentResponse = await _studentResponseRepo.GetByIdAsync(id, includes: nameof(Assignment));
            if (studentResponse == null) throw new NotFoundException("Not found");
            TeacherResponseVm vm = new TeacherResponseVm
            {
                StudentResponse = studentResponse,
            };
            return vm;
        }



        public async Task<bool> TeacherResponseAsync(int id, TeacherResponseVm vm, ModelStateDictionary modelstate)
        {
            if (!modelstate.IsValid) return false;
            StudentResponse studentResponse = await _studentResponseRepo.GetByIdAsync(id, includes: nameof(Assignment));
            if (studentResponse == null) throw new NotFoundException("Not found");
            if (vm.Point > studentResponse.Assignment.MaxPoint)
            {
                modelstate.AddModelError(String.Empty, "Point can't be higher than the maximum points of the assignment");
                return false;
            }
            TeacherResponse teacherresponse = new TeacherResponse
            {
                AssignmentId = studentResponse.AssignmentId,
                StudentId = studentResponse.StudentId,
                Point = vm.Point
            };
            if (vm.Note != null) teacherresponse.Note = vm.Note;
            Student student = await _studentRepo.GetByIdAsync(studentResponse.StudentId);
            if (student == null) throw new NotFoundException("Not found");
            student.Point += vm.Point;
            studentResponse.IsReplied = true;
            _studentRepo.Update(student);
            await _studentRepo.SaveChangesAsync();
            await SaveChangesAsync(studentResponse, teacherresponse);
            return true;
        }
        public async Task<TeacherResponseVm> GetTeacherResponseUpdate(int id)
        {
            StudentResponse studentResponse = await _studentResponseRepo.GetByIdAsync(id, includes: nameof(Assignment));
            if (studentResponse == null) throw new NotFoundException("Not found");
            TeacherResponse teacherResponse = await _teacherResponseRepo.GetByExpressionAsync(x => x.StudentId == studentResponse.StudentId);
            if (teacherResponse == null) throw new NotFoundException("Not found");
            TeacherResponseVm Vm = new TeacherResponseVm
            {
                StudentResponse = studentResponse,
                Point = teacherResponse.Point,
                Note = teacherResponse.Note,
            };
            return Vm;

        }
        public async Task<bool> TeacherResponseUpdate(int id, TeacherResponseVm vm, ModelStateDictionary modelstate)
        {
            if (!modelstate.IsValid) return false;
            StudentResponse studentResponse = await _studentResponseRepo.GetByIdAsync(id, includes: nameof(Assignment));
            if (studentResponse == null) throw new NotFoundException("Not found");
            if (vm.Point > studentResponse.Assignment.MaxPoint)
            {
                modelstate.AddModelError(String.Empty, "Point can't be higher than the maximum points of the assignment");
                return false;
            }
            TeacherResponse exist = await _teacherResponseRepo.GetByExpressionAsync(x => x.StudentId == studentResponse.StudentId);
            if (exist == null) throw new NotFoundException("Not found");
            if (exist.Note != null)
            {   
                exist.Note = vm.Note;
                _teacherResponseRepo.Update(exist);
                await _teacherResponseRepo.SaveChangesAsync();
            }
            if (exist.Point != vm.Point)
            {
                Student student = await _studentRepo.GetByIdAsync(studentResponse.StudentId);
                if (student == null) throw new NotFoundException("Not found");
                student.Point -= exist.Point;
                student.Point += vm.Point;
                exist.Point = vm.Point;
                _studentRepo.Update(student);
                await _studentRepo.SaveChangesAsync();
                _teacherResponseRepo.Update(exist);
                await _teacherResponseRepo.SaveChangesAsync();
            }       
            return true;
        }


        public async Task<UpdateStudenResponseVm> GetUpdateStudenResponse(int id)
        {
			StudentResponse exist = await GetStudentResponseByAssignmentId(id);
			UpdateStudenResponseVm vm = new UpdateStudenResponseVm();
            if(exist.Response!=null) vm.Response = exist.Response;
            if (exist.AttachedFilePath != null)
            {
                string path = exist.AttachedFilePath.Substring(36);
                vm.AttachedFilePath = path;
            }
            vm.Assignment = exist.Assignment;
            return vm;
		}

		public async Task<bool> UpdateStudentResponse(int id, UpdateStudenResponseVm vm, ModelStateDictionary modelstate)
		{
            if(!modelstate.IsValid) return false;			
			StudentResponse exist = await GetStudentResponseByAssignmentId(id);			
			if (vm.Response == null && vm.AttachedFile == null)
			{
				modelstate.AddModelError(String.Empty, "The answer cannot be empty");
				return false;
			}
			if (vm.Response != null)
			{
				exist.Response = vm.Response;
			}
			if (vm.AttachedFilePath == null)
			{
				if (exist.AttachedFilePath != null) exist.AttachedFilePath.DeleteFile(_env.WebRootPath, "uploads");
                exist.AttachedFilePath = null;
			}
			if (vm.AttachedFile != null)
            {
                if (vm.AttachedFile.CheckSize(20))
                {
                    modelstate.AddModelError("AttachedFile", "Maximum file size is 20 mb");
                    return false;
                }
                if(exist.AttachedFilePath!=null) exist.AttachedFilePath.DeleteFile(_env.WebRootPath, "uploads");
				string file = await vm.AttachedFile.CreateFileAsync(_env.WebRootPath, "uploads");
                exist.AttachedFilePath = file;
			}
            exist.UpdateDate = DateTime.Now;
            _studentResponseRepo.Update(exist);
            await _studentResponseRepo.SaveChangesAsync();
            return true;
		}


		//////////////////// ========================= Private Methods =================== ////////////////////////
        ///
		
        private async Task<StudentResponse> GetStudentResponseByAssignmentId(int assignmentid)
        {
			if (assignmentid < 1) throw new BadRequestException("Bad request");
			Assignment assignment = await _repo.GetByIdAsync(assignmentid);
			if (assignment == null) throw new NotFoundException("Not found");
			StudentResponse exist = await _studentResponseRepo.GetByExpressionAsync(x => x.AssignmentId == assignmentid);
			if (exist == null) throw new NotFoundException("Not found");
            return exist;
		}
        private async Task SaveChangesAsync(StudentResponse studentResponse, TeacherResponse teacherresponse)
        {
            _studentResponseRepo.Update(studentResponse);
            await _studentResponseRepo.SaveChangesAsync();
           
            await _teacherResponseRepo.AddAsync(teacherresponse);
            await _teacherResponseRepo.SaveChangesAsync();
        }

		
	}
}
