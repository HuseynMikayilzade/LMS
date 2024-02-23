using LearningManagementSystem.Application.Abstraction;
using LearningManagementSystem.Application.Abstraction.Repositories;
using LearningManagementSystem.Application.Utilities.Exceptions;
using LearningManagementSystem.Application.ViewModels;
using LearningManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Persistance.Implementations
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepo _repo;
       

        public SubjectService(ISubjectRepo repo)
        {
            _repo = repo;
        }
        public async Task<bool> CreateAsync(CreateSubjectVm vm, ModelStateDictionary modelstate)
        {
            if (!modelstate.IsValid) return false;
            if (await _repo.IsExist(l => l.Name == vm.Name))
            {
                modelstate.AddModelError("Name", "This group is already exist");
                return false;
            }
            Subject subject = new Subject
            {
                Name = vm.Name,
                CreateDate = DateTime.UtcNow,
                GroupSubjects = new List<GroupSubject>()
            };       
            await _repo.AddAsync(subject);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id < 1) throw new BadRequestException("Bad request");
            Subject exist = await _repo.GetByIdAsync(id);
            if (exist == null) throw new NotFoundException("Not found");
            _repo.Delete(exist);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<PaginationVm<Subject>> GetAllAsync(int page =1 , int take=10)
        {
            if (page < 1 || take < 1) throw new BadRequestException("Bad request");
            ICollection<Subject> subjects = await _repo.GetAllWhere(skip: (page - 1) * take, take: take,orderexpression:x=>x.Id,isDescending:true,includes: new string[] { "GroupSubjects", "GroupSubjects.Group" }).ToListAsync();
            if (subjects == null) throw new NotFoundException("Not found");
            int count = await _repo.GetAll().CountAsync();
            if (count < 0) throw new NotFoundException("Not found");
            double totalpage = Math.Ceiling((double)count / take);
            PaginationVm<Subject> vm = new PaginationVm<Subject>
            {
                Items = subjects,
                CurrentPage = page,
                TotalPage = totalpage
            };
            return vm;
        }

        public async Task<bool> UpdateAsync(int id, UpdateSubjectVm vm, ModelStateDictionary modelstate)
        {
            if (id < 1) throw new BadRequestException("Bad request");
            if (!modelstate.IsValid) return false;
            Subject exist = await _repo.GetByIdAsync(id);
            if (exist == null) throw new NotFoundException("Not found");
            if (exist.Name != vm.Name)
            {
                if (await _repo.IsExist(l => l.Name == vm.Name))
                {
                    modelstate.AddModelError("Name", "This subject is already exist");
                    return false;
                }
            }
            exist.Name = vm.Name;
            exist.UpdateDate = DateTime.UtcNow;
            _repo.Update(exist);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<UpdateSubjectVm> UpdatedAsync(int id, UpdateSubjectVm vm)
        {
            Subject exist = await _repo.GetByIdAsync(id);
            if (exist == null) throw new NotFoundException("Not found");
            vm.Name = exist.Name;                   
            return vm;
        }
    }
}