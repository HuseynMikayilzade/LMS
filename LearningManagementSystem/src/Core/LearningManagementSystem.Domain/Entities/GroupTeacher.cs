using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.Entities
{
    public class GroupTeacher:BaseEntity
    {
        public Teacher Teacher { get; set; } = null!;
        public int TeacherId { get; set; }
        public Group Group { get; set; } = null!;
        public int GroupId { get; set; }
    }
}