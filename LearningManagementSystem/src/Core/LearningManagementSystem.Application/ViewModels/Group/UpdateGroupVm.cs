using LearningManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.ViewModels
{
    public class UpdateGroupVm
    {
        public string Name { get; set; } = null!;
        public byte MaxStudent { get; set; }     
        public ICollection<int>? RoomIds { get; set; }
        public ICollection<Room>? Rooms { get; set; }
        public ICollection<int>? SubjectIds { get; set; }
        public ICollection<Subject>? Subjects { get; set; }  
    }

}
