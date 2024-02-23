using LearningManagementSystem.Application.ViewModels;
using LearningManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Abstraction.Services
{
    public interface IRoomService
    {
        Task<bool> CreateAsync(CreateRoomVm vm, ModelStateDictionary modelstate);
        Task<bool> UpdateAsync(int id, UpdateRoomVm vm, ModelStateDictionary modelstate);  
        Task<bool> DeleteAsync(int id);
        Task<PaginationVm<Room>> GetAllAsync(int page = 1, int take = 10);
    }
}
