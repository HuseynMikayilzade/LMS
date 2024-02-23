using LearningManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.ViewModels
{
    public class StudentVm
    {
        public List<Student>? Students { get; set; }
        public Dictionary<int,bool> Attendances { get; set; }
    }
}
