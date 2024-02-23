using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.Entities
{
    public class Room:BaseNameableEntity
    {
        public byte Capacity { get; set; }

        // Relation props //
        //public ICollection<GroupRoom>? GroupRooms { get; set; }

    }
}
