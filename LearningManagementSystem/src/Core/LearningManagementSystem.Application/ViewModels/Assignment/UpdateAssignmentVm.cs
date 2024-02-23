using LearningManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.ViewModels
{
    public class UpdateAssignmentVm
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double MaxPoint { get; set; }
        public bool IsActive { get; set; }
        public TaskType TaskType { get; set; }
        public int? GroupId { get; set; }
    }
}
