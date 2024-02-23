using LearningManagementSystem.Application.ViewModels;
using LearningManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LearningManagementSystem.Application.Abstraction.Services
{
    public interface IGroupService
    {
        Task<bool> CreateAsync(CreateGroupVm vm, ModelStateDictionary modelstate);
        Task<CreateGroupVm> CreatedAsync(CreateGroupVm vm);
        Task<bool> UpdateAsync(int id, UpdateGroupVm vm, ModelStateDictionary modelstate);
        Task<UpdateGroupVm> UpdatedAsync(int id, UpdateGroupVm vm);
        Task<bool> DeleteAsync(int id);
        Task<PaginationVm<Group>> GetAllAsync(int page = 1, int take = 10);
        Task<GroupItemVm> GetGroupItems(int id);
    }
}
