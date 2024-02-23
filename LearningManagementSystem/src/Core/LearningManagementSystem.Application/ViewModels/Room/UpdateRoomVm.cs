using LearningManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.ViewModels
{
    public class UpdateRoomVm
    {
        public string Name { get; set; } = null!;
        public byte Capacity { get; set; }
     
    }
}
