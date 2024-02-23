using LearningManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.ViewModels
{
    public class CreateAssignmentVm
    {
        [Required,MaxLength(70)]
        public string Name { get; set; }
        public string? Description { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime StartTime { get; set; } 
        [DataType(DataType.DateTime)]
        public DateTime EndTime { get; set; } 
        public double MaxPoint { get; set; }
        public bool IsActive { get; set; }
        public TaskType TaskType { get; set; }
        public int? GroupId { get; set; }
    }
}
