using LearningManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.Entities
{
    public class Attendance:BaseEntity
    {
        public DateTime Date { get; set; }    
        public int GroupId { get; set; }
        public Group? Group { get; set; }      
        public bool IsPresent { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public string? Comment { get; set; }
        //public ICollection<Student>? Students { get; set; }
    }
}
