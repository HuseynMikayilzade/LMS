using LearningManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace LearningManagementSystem.Application.ViewModels
{
    public class TeacherItemVm
    {
        public ICollection<Group>? Groups { get; set; }
        public Teacher Teacher { get; set; }
        public int TotalStudent { get; set; }
        public int TotalTask { get; set; }
        public int ActiveTask { get; set; }

    }
}
