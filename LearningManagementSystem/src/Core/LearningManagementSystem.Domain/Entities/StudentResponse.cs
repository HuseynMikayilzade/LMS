using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.Entities
{
    public class StudentResponse:BaseEntity
    {       
        public string? Response { get; set; }        
        public string? AttachedFilePath { get; set; }
        public int AssignmentId { get; set; }
        public int StudentId { get; set; }
        public Student? Student { get; set; }
        public Assignment? Assignment { get; set; }
        public bool IsSended { get; set; } = false;
        public bool IsReplied { get; set; } = false;
    }
}
