using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.Entities
{
    public class TeacherResponse:BaseEntity
    {        
        public int StudentId { get; set; }
        public Student? Student { get; set; }
        public int AssignmentId { get; set; }
        public Assignment? Assignment { get; set; }
        public double Point { get; set; }
        public string? Note { get; set; }
    }
}
