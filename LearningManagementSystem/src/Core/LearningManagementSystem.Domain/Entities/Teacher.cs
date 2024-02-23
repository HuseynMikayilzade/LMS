using LearningManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.Entities
{
    public class Teacher:BaseNameableEntity
    {
        public string Surname { get; set; } = null!;
        public Gender Gender { get; set; }
        public string? Image { get; set; } = "profil.png";
        public DateTime Birthday { get; set; }
        public string EmailAddress { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public Subject? Subject { get; set; }
        public int? SubjectId { get; set; }
        public ICollection<GroupTeacher>? GroupTeachers { get; set; }
        public ICollection<StudentTeacher>? StudentTeachers { get; set; }
       
    }
}
