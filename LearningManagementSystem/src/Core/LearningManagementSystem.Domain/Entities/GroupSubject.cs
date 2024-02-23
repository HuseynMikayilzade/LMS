using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.Entities
{
    public class GroupSubject:BaseEntity
    {
        public Group Group { get; set; } = null!;
        public int GroupId { get; set; }
        public Subject Subject { get; set; } = null!;
        public int SubjectId { get; set; }
    }
}
