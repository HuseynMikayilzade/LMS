using LearningManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.ViewModels
{
    public class GroupItemVm
    {
        public ICollection<Student>? Students { get; set; }
        public ICollection<Assignment>? Assignments { get; set; }
        public Group Group { get; set; }
        public int AssignmentCount { get; set; }
        public int StudentCount { get; set; }
        public List<AttendanceVm>? AttendanceVm { get; set; }
        public List<Attendance>? Attendances { get; set; }
        public PaginationVm<Attendance>? PaginationAttendances { get; set; }

    }
}
