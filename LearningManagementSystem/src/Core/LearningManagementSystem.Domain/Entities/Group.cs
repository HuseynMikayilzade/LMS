using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.Entities
{
    public class Group:BaseNameableEntity
    {
        public byte MaxStudent { get; set; }
        public byte CurrentStudent { get; set; }
        public ICollection<Student>? Students { get; set; }
        public ICollection<GroupSubject>? GroupSubjects { get; set; }
        public ICollection<GroupTeacher>? GroupTeachers { get; set; }
        public ICollection<GroupRoom>? GroupRooms { get; set; }
    }
}