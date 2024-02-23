using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.Entities
{
    public class StudentTeacher:BaseEntity
    {
        public Teacher Teacher { get; set; } = null!;
        public int TeacherId { get; set; }
        public Student student { get; set; } = null!;
        public int StudentId { get; set; }
    }
}
