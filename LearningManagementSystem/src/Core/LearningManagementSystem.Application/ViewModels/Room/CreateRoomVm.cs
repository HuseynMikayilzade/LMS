using LearningManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.ViewModels
{
    public class CreateRoomVm
    {
        public string Name { get; set; } = null!;
        public byte Capacity { get; set; }

        //public ICollection<int>? GroupIds { get; set; }
        //public ICollection<Group>? Groups { get; set; }
    }
}
