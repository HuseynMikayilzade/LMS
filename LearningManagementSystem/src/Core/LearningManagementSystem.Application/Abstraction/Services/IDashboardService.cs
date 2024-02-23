using LearningManagementSystem.Application.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Abstraction.Services
{
    public interface IDashboardService
    {
        Task<DashboardVm> GetAllAsync();
    }
}
