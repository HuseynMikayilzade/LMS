using LearningManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.Entities
{
    public class Assignment:BaseNameableEntity
    {
        public string? Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double MaxPoint { get; set; }
        public bool IsActive { get; set; }
        public TaskType TaskType { get; set; }
        public int GroupId { get; set; }
        public Group? Group { get; set; }
        public ICollection<StudentResponse>? StudentResponses { get; set; }
      
    }
}