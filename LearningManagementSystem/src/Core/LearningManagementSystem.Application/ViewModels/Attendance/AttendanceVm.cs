using LearningManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.ViewModels
{
    public class AttendanceVm
    {
        public Student? Student { get; set; }
        public bool IsPresent { get; set; }
        public string? Comment { get; set; }
        public int StudentId { get; set; }
        public int GroupId { get; set; }

    }
}
