using LearningManagementSystem.Application.Abstraction.Repositories;
using LearningManagementSystem.Application.Abstraction.Services;
using LearningManagementSystem.Application.Utilities.Exceptions;
using LearningManagementSystem.Application.ViewModels;
using LearningManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Persistance.Implementations.Services
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepo _repo;
        private readonly ISubjectRepo _subjectRepo;

        public LessonService(ILessonRepo repo, ISubjectRepo subjectRepo)
        {
            _repo = repo;
            _subjectRepo = subjectRepo;
        }
        public async Task<PaginationVm<Lesson>> GetAllAsync(int page = 1, int take = 10)
        {
            if (page < 1 || take < 1) throw new BadRequestException("Bad request");
            ICollection<Lesson> lessons = await _repo.GetAllWhere(skip: (page - 1) * take, take: take, orderexpression: x => x.Id, isDescending: true, includes: nameof(Subject)).ToListAsync();
            if(lessons==null) throw new NotFoundException("Not found");
            int count = await _repo.GetAll().CountAsync();
            if (count < 0) throw new NotFoundException("Not found");
            double totalpage = Math.Ceiling((double)count / take);
            PaginationVm<Lesson> vm = new PaginationVm<Lesson>
            {
                Items = lessons,
                CurrentPage = page,
                TotalPage = totalpage
            };
            return vm;

        }
        public async Task<bool> CreateAsync(CreateLessonVm vm, ModelStateDictionary modelstate)
        {
            if (!modelstate.IsValid) return false;
            if (await _repo.IsExist(l => l.Name == vm.Name))
            {
                modelstate.AddModelError("Name", "This lesson is already exist");
                return false;
            }
            if (!await _subjectRepo.IsExist(s => s.Id == vm.SubjectId))
            {
                modelstate.AddModelError("SubjectId", "This Subject is not aviable");
                return false;
            }
            if (vm.StartDate >= vm.EndDate)
            {
                modelstate.AddModelError("EndDate", "End Date must be greater than Start Date");
                return false;
            }
            Lesson lesson = new Lesson
            {
                Name = vm.Name.Trim(),
                Date = vm.Date,
                Duration = vm.EndDate - vm.StartDate,
                EndDate = vm.EndDate,
                StartDate = vm.StartDate,
                Tittle = vm.Tittle.Trim(),
                SubjectId = vm.SubjectId,
            };
            await _repo.AddAsync(lesson);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<CreateLessonVm> CreatedAsync(CreateLessonVm vm)
        {
            vm.Subjects = await _subjectRepo.GetAll().ToListAsync();
            return vm;
        }
        public async Task<bool> UpdateAsync(int id, UpdateLessonVm vm, ModelStateDictionary modelstate)
        {
            if (id < 1) throw new BadRequestException("Bad request");
            if (!modelstate.IsValid) return false;
            Lesson exist = await _repo.GetByIdAsync(id);
            if (exist == null) throw new NotFoundException("Not found");
            if(exist.Name != vm.Name)
            {
                if (await _repo.IsExist(l => l.Name == vm.Name))
                {
                    modelstate.AddModelError("Name", "This lesson is already exist");
                    return false;
                }
            }     
            if (!await _subjectRepo.IsExist(s => s.Id == vm.SubjectId))
            {
                modelstate.AddModelError("SubjectId", "This Subject is not aviable");
                return false;
            }
            if (vm.StartDate >= vm.EndDate)
            {
                modelstate.AddModelError("EndDate", "End Date must be greater than Start Date");
                return false;
            }
            exist.Tittle = vm.Tittle.Trim();
            exist.Name = vm.Name.Trim();
            exist.StartDate = vm.StartDate;
            exist.EndDate = vm.EndDate;
            exist.Date = vm.Date;
            exist.Duration = vm.EndDate - vm.StartDate;
            exist.UpdateDate = DateTime.UtcNow;
            exist.SubjectId = vm.SubjectId;
            _repo.Update(exist);
            await _repo.SaveChangesAsync();
            return true;
        }
        public async Task<UpdateLessonVm> UpdatedAsync(int id, UpdateLessonVm vm)
        {
            if (id < 1) throw new BadRequestException("Bad request");
            Lesson exist = await _repo.GetByIdAsync(id);
            if (exist == null) throw new NotFoundException("Not found"); 
            vm.Subjects = await _subjectRepo.GetAll().ToListAsync();
            vm.Tittle = exist.Tittle.Trim();
            vm.Name = exist.Name.Trim();
            vm.StartDate = exist.StartDate;
            vm.EndDate = exist.EndDate;
            vm.Date = exist.Date;
            vm.SubjectId = exist.SubjectId;
            return vm;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id < 1) throw new BadRequestException("Bad request");
            Lesson exist = await _repo.GetByIdAsync(id);
            if (exist == null) throw new NotFoundException("Not found");
            _repo.Delete(exist);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
