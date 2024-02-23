using LearningManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.ViewModels
{
    public class CreateAttendanceVm
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public List<int> StudentIds { get; set; }
        public List<bool> IsPresents { get; set; }
        public List<string>? Comments { get; set; }
        public List<AttendanceVm>? AttendanceVm { get; set; }
    }
}
    