
using LearningManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.ViewModels
{
    public class DashboardVm
    {
        public int StudentCount { get; set; }
        public int TeacherCount { get; set; }
        public int GroupCount { get; set; }
        public ICollection<Student>? StarStudents { get; set; }
    }
}
