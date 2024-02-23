using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.Entities
{
    public class GroupRoom:BaseEntity
    {
        public int GroupId { get; set; }
        public Group Group { get; set; } = null!;
        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;
    }
}
