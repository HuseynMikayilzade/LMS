using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.Entities
{
    public class TeacherResponse:BaseEntity
    {        
        public int AssignmentId { get; set; }
        public Assignment? Assignment { get; set; }
        public int StudentResponseId { get; set; }
        public StudentResponse? StudentResponse { get; set; }
        public double Point { get; set; }
        public string? Note { get; set; }
    }
}
