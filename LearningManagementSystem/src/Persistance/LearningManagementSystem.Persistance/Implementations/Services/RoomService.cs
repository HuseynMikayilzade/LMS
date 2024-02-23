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
    public class RoomService : IRoomService
    {
        private readonly IRoomRepo _repo;

        public RoomService(IRoomRepo roomRepo)
        {
            _repo = roomRepo;
        }
        public async Task<bool> CreateAsync(CreateRoomVm vm, ModelStateDictionary modelstate)
        {
            if (!modelstate.IsValid) return false;
            if (await _repo.IsExist(l => l.Name == vm.Name))
            {
                modelstate.AddModelError("Name", "This room is already exist");
                return false;
            }
            Room room = new Room
            {
                Name = vm.Name,
                Capacity = vm.Capacity,
                CreateDate = DateTime.UtcNow,             
            };        
            await _repo.AddAsync(room);
            await _repo.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            if (id < 1) throw new BadRequestException("Bad request");
            Room exist = await _repo.GetByIdAsync(id);
            if (exist == null) throw new NotFoundException("Not found");
            _repo.Delete(exist);
            await _repo.SaveChangesAsync();
            return true;
        }
        public async Task<PaginationVm<Room>> GetAllAsync(int page = 1, int take = 10)
        {
            if (page < 1 || take < 1) throw new BadRequestException("Bad request");
            ICollection<Room> rooms = await _repo.GetAllWhere(skip: (page - 1) * take, take: take,orderexpression:x=>x.Id,isDescending:true).ToListAsync();
            if (rooms == null) throw new NotFoundException("Not found");
            int count = await _repo.GetAll().CountAsync();
            if (count < 0) throw new NotFoundException("Not found");
            double totalpage = Math.Ceiling((double)count / take);
            PaginationVm<Room> vm = new PaginationVm<Room>
            {
                Items = rooms,
                CurrentPage = page,
                TotalPage = totalpage
            };
            return vm;
        }
        public async Task<bool> UpdateAsync(int id, UpdateRoomVm vm, ModelStateDictionary modelstate)
        {
            if (id < 1) throw new BadRequestException("Bad request");
            if (!modelstate.IsValid) return false;
            Room exist = await _repo.GetByIdAsync(id);
            if (exist == null) throw new NotFoundException("Not found");
            if (exist.Name != vm.Name)
            {
                if (await _repo.IsExist(l => l.Name == vm.Name))
                {
                    modelstate.AddModelError("Name", "This room is already exist");
                    return false;
                }
            }          
            exist.Capacity = vm.Capacity;
            exist.Name = vm.Name;
            exist.UpdateDate = DateTime.UtcNow;
            _repo.Update(exist);
            await _repo.SaveChangesAsync();
            return true;
        }
        public async Task<UpdateRoomVm> UpdatedAsync(int id, UpdateRoomVm vm)
        {
            Room exist = await _repo.GetByIdAsync(id);
            if (exist == null) throw new NotFoundException("Not found");
            vm.Name = exist.Name;
            vm.Capacity = exist.Capacity;      
            return vm;
        }
    }
}
